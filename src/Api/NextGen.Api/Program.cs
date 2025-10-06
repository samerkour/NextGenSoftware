using System;
using System.Net;
using System.Reflection;
using System.Text.Json;
using Amazon.S3;
using AspNetCoreRateLimit; // Added for rate limiting
using BuildingBlocks.Core.Extensions;
using BuildingBlocks.Core.Extensions.ServiceCollection;
using BuildingBlocks.Logging;
using BuildingBlocks.Monitoring;
using BuildingBlocks.Security;
using BuildingBlocks.Security.Extensions;
using BuildingBlocks.Security.Jwt;
using BuildingBlocks.Swagger;
using BuildingBlocks.Validation;
using BuildingBlocks.Web;
using BuildingBlocks.Web.Extensions;
using BuildingBlocks.Web.Extensions.ServiceCollectionExtensions;
using BuildingBlocks.Web.Module;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using NextGen.Api;
using NextGen.Api.Extensions.ApplicationBuilderExtensions;
using NextGen.Api.Extensions.ServiceCollectionExtensions;
using NextGen.Modules.Identity;
using NextGen.Modules.Inventories;
using NextGen.Modules.Parties;
using NextGen.Modules.Sales;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

RegisterServices(builder);

var app = builder.Build();

await ConfigureApplication(app);

await app.RunAsync();

static void RegisterServices(WebApplicationBuilder builder)
{
    builder.Host.UseDefaultServiceProvider((env, c) =>
    {
        // Handling Captive Dependency Problem
        // https://ankitvijay.net/2020/03/17/net-core-and-di-beware-of-captive-dependency/
        // https://levelup.gitconnected.com/top-misconceptions-about-dependency-injection-in-asp-net-core-c6a7afd14eb4
        // https://blog.ploeh.dk/2014/06/02/captive-dependency/
        if (env.HostingEnvironment.IsDevelopment()
            || env.HostingEnvironment.IsEnvironment("test")
            || env.HostingEnvironment.IsStaging())
        {
            c.ValidateScopes = true;
        }
    });

    builder.Configuration.AddModulesSettingsFile(
        builder.Environment.ContentRootPath,
        builder.Environment.EnvironmentName);

    // https://www.michaco.net/blog/EnvironmentVariablesAndConfigurationInASPNETCoreApps#environment-variables-and-configuration
    // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-6.0#non-prefixed-environment-variables
    builder.Configuration.AddEnvironmentVariables("NextGen_env_");

    // Register AmazonS3Client for MinIO/S3 using configuration
    builder.Services.AddSingleton<IAmazonS3>(sp =>
    {
        var configuration = sp.GetRequiredService<IConfiguration>();
        var s3Section = configuration.GetSection("S3");

        var accessKey = s3Section.GetValue<string>("AccessKey");
        var secretKey = s3Section.GetValue<string>("SecretKey");
        var serviceUrl = s3Section.GetValue<string>("BaseUrl");
        var region = s3Section.GetValue<string>("Region");

        return new AmazonS3Client(
            accessKey,
            secretKey,
            new AmazonS3Config
            {
                ServiceURL = serviceUrl,     // MinIO / S3 endpoint
                //RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(region),
                ForcePathStyle = true  // required for MinIO, safe for AWS S3
            });
    });

    // https://github.com/tonerdo/dotnet-env
    DotNetEnv.Env.TraversePath().Load();

    builder.Services.AddHttpContextAccessor();

    builder.Services.AddNextGenMonitoring(builder.Configuration);

    builder.Services.AddApplicationOptions(builder.Configuration);
    var loggingOptions = builder.Configuration.GetSection(nameof(LoggerOptions)).Get<LoggerOptions>();

    builder.Host.AddCustomSerilog(
        optionsBuilder =>
        {
            optionsBuilder.SetLevel(LogEventLevel.Information);
        },
        config =>
        {
            config.WriteTo.File(
                NextGen.Api.Program.GetLogPath(builder.Environment, loggingOptions) ??
                "../logs/parties-service.log",
                outputTemplate: loggingOptions?.LogTemplate ??
                                "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} {Level} - {Message:lj}{NewLine}{Exception}",
                rollingInterval: RollingInterval.Day,
                rollOnFileSizeLimit: true);
        });

    /*----------------- Module Services Setup ------------------*/
    builder.AddModulesServices(builder.Environment, useCompositionRootForModules: true);

    // https://andrewlock.net/controller-activation-and-dependency-injection-in-asp-net-core-mvc/
    // Configure MVC to use custom response format
    builder.Services.AddControllers(options =>
    {
        options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
        // Add custom filter for response formatting
        options.Filters.Add<ApiResponseFilter>();
    }).AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
        .ConfigureApiBehaviorOptions(options =>
        {
            // Suppress default ProblemDetails for model validation errors
            options.SuppressMapClientErrors = true;
            options.SuppressModelStateInvalidFilter = true;
        });

    builder.Services.ReplaceTransient<IControllerActivator, CustomServiceBasedControllerActivator>();

    builder.Services.AddCustomProblemDetails();
    builder.Services.AddCompression();

    builder.Services.AddCustomVersioning();
    builder.AddCustomSwagger(new[]
    {
        typeof(InventoryRoot).Assembly, typeof(IdentityRoot).Assembly, typeof(SalesRoot).Assembly,
        typeof(PartiesRoot).Assembly,
    });

    builder.Services.AddCustomJwtAuthentication(builder.Configuration);
    builder.Services.AddCustomAuthorization(
        rolePolicies: new List<RolePolicy>
        {
            new(ApiConstants.Role.Admin, new List<string> {ApiConstants.Role.SecurityAdmin}),
            new(ApiConstants.Role.Admin, new List<string> {ApiConstants.Role.Admin}),
            new(ApiConstants.Role.User, new List<string> {ApiConstants.Role.User})
        });

    // Add rate limiting services
    builder.Services.AddMemoryCache(); // Required for in-memory rate limiting
    builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>(); // Added for rate limiting
    builder.Services.AddInMemoryRateLimiting(); // Use in-memory store for simplicity
    builder.Services.Configure<IpRateLimitOptions>(options =>
    {
        options.GeneralRules = new List<RateLimitRule>
        {
            new RateLimitRule
            {
                Endpoint = "POST:/api/v1/identity/login", // Target login endpoint (v1)
                Limit = 5, // 5 requests
                Period = "1m" // per minute
            },
            new RateLimitRule
            {
                Endpoint = "POST:/api/v2/identity/login", // Target login endpoint (v2)
                Limit = 5,
                Period = "1m"
            }
        };
        options.QuotaExceededResponse = new QuotaExceededResponse
        {
            ContentType = "application/json",
            Content = "{\"error\": \"Rate limit exceeded. Try again later.\"}",
            StatusCode = 429 // Too Many Requests
        };
    });
    builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
    builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

    // Register custom response service
    builder.Services.AddTransient<IApiResponseService, ApiResponseService>();
}

static async Task ConfigureApplication(WebApplication app)
{
    var environment = app.Environment;

    app.UseProblemDetails();

    app.UseSerilogRequestLogging();

    // Add rate limiting middleware before routing
    app.UseIpRateLimiting(); // Added for rate limiting

    app.UseRouting();
    app.UseAppCors();

    // Add custom response formatting middleware
    app.UseMiddleware<ApiResponseMiddleware>();

    app.UseAuthentication();
    app.UseAuthorization();

    /*----------------- Module Middleware Setup ------------------*/
    await app.ConfigureModules();

    app.MapControllers();

    /*----------------- Module Routes Setup ------------------*/
    app.MapModulesEndpoints();

    // automatic discover minimal endpoints
    app.MapEndpoints(typeof(ApiRoot).Assembly);

    app.MapGet("/", (HttpContext _) => "Next Gen Modular Monolith Api.").ExcludeFromDescription();

    if (environment.IsDevelopment() || environment.IsEnvironment("docker"))
    {
        // swagger middleware should register after register endpoints to discover all endpoints and its versions correctly
        app.UseCustomSwagger();
    }

    app.UseNextGenMonitoring();

    app.Lifetime.ApplicationStopping.Register(() =>
    {
        if (app.Environment.IsEnvironment("test") == false)
        {
            foreach (var compositionRoot in CompositionRootRegistry.CompositionRoots)
            {
                compositionRoot.ServiceProvider.StopHostedServices().GetAwaiter().GetResult();
            }
        }
    });

    Log.Logger = new LoggerConfiguration()
        .WriteTo.Console()
        .CreateBootstrapLogger();
}

namespace NextGen.Api
{
    public partial class Program
    {
        public static string? GetLogPath(IWebHostEnvironment env, LoggerOptions loggerOptions)
            => env.IsDevelopment() ? loggerOptions.DevelopmentLogPath : loggerOptions.ProductionLogPath;
    }

    // Custom response classes

    public class ApiResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public int StatusCode { get; set; }
        public string ApiVersion { get; set; } = "1.0";
        public object? Data { get; set; }

        public ApiResponse(bool success, object? data, string? message, int statusCode, string? apiVersion = null)
        {
            Success = success;
            Data = data;
            Message = message;
            StatusCode = statusCode;
            ApiVersion = apiVersion ?? "1.0";
        }

        public static ApiResponse SuccessResponse(object? data = null, string? message = "Request successful.", int statusCode = 200, string? apiVersion = null)
            => new(success: true, data, message, statusCode, apiVersion);
    }

    public class ApiErrorResponse
    {
        public bool Success { get; set; } = false;
        public int StatusCode { get; set; }
        public string ApiVersion { get; set; } = "1.0";
        public object Error { get; set; }

        public ApiErrorResponse(int statusCode, object error, string? apiVersion = null)
        {
            StatusCode = statusCode;
            Error = error;
            ApiVersion = apiVersion ?? "1.0";
        }
    }

    public class ErrorDetails
    {
        public string Message { get; set; }

        public ErrorDetails(string message)
        {
            Message = message;
        }
    }

    // Middleware for response formatting
    public class ApiResponseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiResponseMiddleware> _logger;

        public ApiResponseMiddleware(RequestDelegate next, ILogger<ApiResponseMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;

            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            try
            {
                await _next(context);

                if (ShouldSkipFormatting(context))
                {
                    await CopyStreamToOriginal(responseBody, originalBodyStream);
                    return;
                }

                await FormatResponseAsync(context, responseBody, originalBodyStream);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, originalBodyStream);
            }
        }

        private bool ShouldSkipFormatting(HttpContext context)
        {
            var path = context.Request.Path.Value ?? "";
            return path.StartsWith("/swagger") || path.StartsWith("/health") || path == "/";
        }

        private async Task FormatResponseAsync(HttpContext context, MemoryStream responseBody, Stream originalBodyStream)
        {
            responseBody.Seek(0, SeekOrigin.Begin);
            var responseContent = await new StreamReader(responseBody).ReadToEndAsync();

            // If already wrapped, forward as-is
            if (!string.IsNullOrEmpty(responseContent) &&
                responseContent.Contains("\"success\":") && responseContent.Contains("\"apiVersion\":"))
            {
                await CopyStreamToOriginal(responseBody, originalBodyStream);
                return;
            }

            var success = context.Response.StatusCode >= 200 && context.Response.StatusCode < 300;
            var apiVersion = GetApiVersionFromRequest(context);

            if (success)
            {
                object? data = null;

                try
                {
                    if (!string.IsNullOrEmpty(responseContent))
                        data = JsonSerializer.Deserialize<object>(responseContent);
                }
                catch
                {
                    data = responseContent;
                }

                var message = context.Response.StatusCode switch
                {
                    200 => "Request successful.",
                    201 => "Resource created successfully.",
                    204 => "No content.",
                    _ => "Request successful."
                };

                var apiResponse = ApiResponse.SuccessResponse(data, message, context.Response.StatusCode, apiVersion);

                await WriteJsonResponse(context, apiResponse, originalBodyStream);
            }
            else
            {
                object errorPayload = new { message = "An error occurred." };

                try
                {
                    if (!string.IsNullOrEmpty(responseContent))
                    {
                        var errorObj = JsonSerializer.Deserialize<JsonElement>(responseContent);

                        //// 🟡 Handle FluentValidation or ASP.NET Validation format:
                        //// Example: {"Email":["'Email' is not a valid email address."]}
                        //if (context.Response.StatusCode == StatusCodes.Status422UnprocessableEntity ||
                        //    (errorObj.ValueKind == JsonValueKind.Object && errorObj.EnumerateObject().Any(p => p.Value.ValueKind == JsonValueKind.Array)))
                        //{
                        //    var validationErrors = new List<ValidationError>();

                        //    foreach (var prop in errorObj.EnumerateObject())
                        //    {
                        //        if (prop.Value.ValueKind == JsonValueKind.Array)
                        //        {
                        //            foreach (var msg in prop.Value.EnumerateArray())
                        //            {
                        //                validationErrors.Add(new ValidationError(prop.Name, msg.GetString() ?? "Invalid value"));
                        //            }
                        //        }
                        //    }

                        //    errorPayload = new
                        //    {
                        //        message = "Validation Failed.",
                        //        errors = validationErrors
                        //            .GroupBy(e => e.Field ?? "General")
                        //            .ToDictionary(g => g.Key, g => g.Select(e => e.Message).ToArray())
                        //    };

                        //    //// ✅ Ensure correct status code
                        //    //context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
                        //}

                        // 🟢 Handle ASP.NET ModelState error format (with "errors" object)
                        // else
                        if (errorObj.TryGetProperty("errors", out var errorsProp) && errorsProp.ValueKind == JsonValueKind.Object)
                        {
                            var validationErrors = new List<ValidationError>();

                            foreach (var prop in errorsProp.EnumerateObject())
                            {
                                foreach (var msg in prop.Value.EnumerateArray())
                                {
                                    validationErrors.Add(new ValidationError(prop.Name, msg.GetString() ?? "Invalid value"));
                                }
                            }

                            errorPayload = new
                            {
                                message = "Validation Failed.",
                                errors = validationErrors
                                    .GroupBy(e => e.Field ?? "General")
                                    .ToDictionary(g => g.Key, g => g.Select(e => e.Message).ToArray())
                            };

                            //// ✅ Set 422 explicitly for validation problems
                            //context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
                        }

                        // 🟠 Generic error response (ProblemDetails, etc.)
                        else if (errorObj.TryGetProperty("title", out var titleProp))
                        {
                            errorPayload = new { message = titleProp.GetString() ?? "Bad request." };
                        }
                        else if (errorObj.TryGetProperty("detail", out var detailProp))
                        {
                            errorPayload = new { message = detailProp.GetString() ?? "An error occurred." };
                        }
                        else if (errorObj.ValueKind == JsonValueKind.String)
                        {
                            errorPayload = new { message = errorObj.GetString() };
                        }
                    }
                }
                catch
                {
                    // Keep default errorPayload if parsing fails
                }

                // 🧩 Build and write unified API error response
                var apiErrorResponse = new ApiErrorResponse(context.Response.StatusCode, errorPayload, apiVersion);
                await WriteJsonResponse(context, apiErrorResponse, originalBodyStream);


            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception, Stream originalBodyStream)
        {
            context.Response.Body = originalBodyStream;
            context.Response.ContentType = "application/json";

            var apiVersion = GetApiVersionFromRequest(context);

            // Handle ValidationException
            if (exception is ValidationException validationException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                var errorResponse = new ApiErrorResponse(
                    context.Response.StatusCode,
                    new
                    {
                        message = validationException.ValidationResult.Message,
                        errors = validationException.ValidationResult.Errors
                            ?.GroupBy(e => e.Field ?? "General")
                            .ToDictionary(g => g.Key, g => g.Select(e => e.Message).ToArray())
                    },
                    apiVersion
                );

                await WriteJsonResponse(context, errorResponse, originalBodyStream);
                return;
            }

            // Map other exceptions
            context.Response.StatusCode = exception switch
            {
                UnauthorizedAccessException => 401,
                KeyNotFoundException => 404,
                _ => 500
            };

            var apiError = new ApiErrorResponse(
                context.Response.StatusCode,
                new { message = exception.Message },
                apiVersion
            );

            await WriteJsonResponse(context, apiError, originalBodyStream);
        }

        private async Task WriteJsonResponse(HttpContext context, object response, Stream originalBodyStream)
        {
            context.Response.Body = originalBodyStream;
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 200;

            context.Response.Headers.ContentLength = null;
            context.Response.Headers.Remove("Content-Length");

            var jsonResponse = JsonSerializer.Serialize(
                response,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = context.RequestServices.GetService<IWebHostEnvironment>().IsDevelopment()
                });

            await context.Response.WriteAsync(jsonResponse);
        }

        private string GetApiVersionFromRequest(HttpContext context)
        {
            var path = context.Request.Path.Value ?? "";
            if (path.Contains("/api/v"))
            {
                var versionStart = path.IndexOf("/api/v", StringComparison.Ordinal) + 6;
                var versionEnd = path.IndexOf('/', versionStart);
                if (versionEnd == -1)
                    versionEnd = path.Length;

                if (versionStart < versionEnd)
                    return path.Substring(versionStart, versionEnd - versionStart);
            }

            if (context.Request.Headers.TryGetValue("api-version", out var versionHeader))
                return versionHeader.ToString();

            return "1.0";
        }

        private async Task CopyStreamToOriginal(MemoryStream source, Stream destination)
        {
            source.Seek(0, SeekOrigin.Begin);
            await source.CopyToAsync(destination);
            source.Seek(0, SeekOrigin.Begin);
        }
    }

    // Action filter for controller responses
    public class ApiResponseFilter : IActionFilter, IResultFilter
    {
        private readonly IApiResponseService _responseService;

        public ApiResponseFilter(IApiResponseService responseService)
        {
            _responseService = responseService;
        }

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult objectResult)
            {
                context.Result = _responseService.FormatResponse(objectResult.Value, context);
            }
        }

        public void OnResultExecuting(ResultExecutingContext context) { }
        public void OnResultExecuted(ResultExecutedContext context) { }
    }

    // Service for response formatting
    public interface IApiResponseService
    {
        IActionResult FormatResponse(object? data, ActionContext context);
    }

    public class ApiResponseService : IApiResponseService
    {
        public IActionResult FormatResponse(object? data, ActionContext context)
        {
            var statusCode = context.HttpContext.Response.StatusCode;
            var success = statusCode >= 200 && statusCode < 300;
            var apiVersion = GetApiVersionFromRequest(context.HttpContext);

            if (success)
                return new ObjectResult(ApiResponse.SuccessResponse(data, null, statusCode, apiVersion)) { StatusCode = 200 };

            var errorMessage = statusCode switch
            {
                400 => "Bad request",
                401 => "Unauthorized",
                403 => "Forbidden",
                404 => "Not found",
                500 => "Internal server error",
                _ => "An error occurred"
            };

            return new ObjectResult(new ApiErrorResponse(statusCode, new { message = errorMessage }, apiVersion)) { StatusCode = 200 };
        }

        private string GetApiVersionFromRequest(HttpContext context)
        {
            var path = context.Request.Path.Value ?? "";
            if (path.Contains("/api/v"))
            {
                var versionStart = path.IndexOf("/api/v", StringComparison.Ordinal) + 6;
                var versionEnd = path.IndexOf('/', versionStart);
                if (versionEnd == -1)
                    versionEnd = path.Length;

                if (versionStart < versionEnd)
                    return path.Substring(versionStart, versionEnd - versionStart);
            }

            if (context.Request.Headers.TryGetValue("api-version", out var versionHeader))
                return versionHeader.ToString();

            return "1.0";
        }
    }

}
