using System;
using System.Globalization;
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
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
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

    builder.Configuration.AddEnvironmentVariables("NextGen_env_");

    builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

    builder.Services.Configure<RequestLocalizationOptions>(options =>
    {
        var supportedCultures = new[]
        {
            new CultureInfo("en"),
            new CultureInfo("fa-IR"),
            new CultureInfo("ar-SA"),
            new CultureInfo("zh-CN")
        };

        options.DefaultRequestCulture = new RequestCulture("en");
        options.SupportedCultures = supportedCultures;
        options.SupportedUICultures = supportedCultures;

        // 🔹 Accept-Language support
        options.RequestCultureProviders.Insert(0, new AcceptLanguageHeaderRequestCultureProvider());

        // 🔹 Optional fallback (fa → fa-IR)
        options.FallBackToParentCultures = true;
        options.FallBackToParentUICultures = true;
    });

    // -------------------------------------------------------

    // S3 registration
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
                ServiceURL = serviceUrl,
                ForcePathStyle = true
            });
    });

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

    builder.Services.AddControllers(options =>
    {
        options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
        options.Filters.Add<ApiResponseFilter>();
    })
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
    // Enable DataAnnotation localization
    .AddDataAnnotationsLocalization();

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
            new(ApiConstants.Role.SecurityAdmin, new List<string> {ApiConstants.Role.SecurityAdmin}),
            new(ApiConstants.Role.Admin, new List<string> {ApiConstants.Role.Admin}),
            new(ApiConstants.Role.User, new List<string> {ApiConstants.Role.User})
        });

    // Rate limiting
    builder.Services.AddMemoryCache();
    builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
    builder.Services.AddInMemoryRateLimiting();
    builder.Services.Configure<IpRateLimitOptions>(options =>
    {
        options.GeneralRules = new List<RateLimitRule>
        {
            new RateLimitRule
            {
                Endpoint = "POST:/api/v1/identity/login",
                Limit = 5,
                Period = "1m"
            },
            new RateLimitRule
            {
                Endpoint = "POST:/api/v2/identity/login",
                Limit = 5,
                Period = "1m"
            }
        };
        options.QuotaExceededResponse = new QuotaExceededResponse
        {
            ContentType = "application/json",
            Content = "{\"error\": \"Rate limit exceeded. Try again later.\"}",
            StatusCode = 429
        };
    });
    builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
    builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

    builder.Services.AddTransient<IApiResponseService, ApiResponseService>();
}

static async Task ConfigureApplication(WebApplication app)
{
    var environment = app.Environment;

    app.UseProblemDetails();
    app.UseSerilogRequestLogging();

    app.UseIpRateLimiting();

    // ---------- Step 2: Use Localization Middleware ----------
    var locOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;
    app.UseRequestLocalization(locOptions);
    // ---------------------------------------------------------

    app.UseRouting();
    app.UseAppCors();

    app.UseMiddleware<ApiResponseMiddleware>();

    app.UseAuthentication();
    app.UseAuthorization();

    await app.ConfigureModules();

    app.MapControllers();
    app.MapModulesEndpoints();

    app.MapEndpoints(typeof(ApiRoot).Assembly);

    app.MapGet("/", (HttpContext _) => "Next Gen Modular Monolith Api.").ExcludeFromDescription();

    if (environment.IsDevelopment() || environment.IsEnvironment("docker"))
    {
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

    // --------------------- ApiResponse Classes ---------------------
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

        public static ApiResponse SuccessResponse(object? data = null, string? message = null, int statusCode = 200, string? apiVersion = null)
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
        public ErrorDetails(string message) => Message = message;
    }

    // --------------------- Middleware for Localization + Formatting ---------------------

    public class ApiResponseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiResponseMiddleware> _logger;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public ApiResponseMiddleware(RequestDelegate next, ILogger<ApiResponseMiddleware> logger, IStringLocalizer<SharedResource> localizer)
        {
            _next = next;
            _logger = logger;
            _localizer = localizer;
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

            if (!string.IsNullOrEmpty(responseContent) &&
                responseContent.Contains("\"success\":") && responseContent.Contains("\"apiVersion\":"))
            {
                await CopyStreamToOriginal(responseBody, originalBodyStream);
                return;
            }

            var success = context.Response.StatusCode is >= 200 and < 300;
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

                // 🟢 Localized messages for success codes
                var message = context.Response.StatusCode switch
                {
                    200 => _localizer["RequestSuccessful"],
                    201 => _localizer["ResourceCreated"],
                    204 => _localizer["NoContent"],
                    _ => _localizer["RequestSuccessful"]
                };

                var apiResponse = ApiResponse.SuccessResponse(data, message, context.Response.StatusCode, apiVersion);
                await WriteJsonResponse(context, apiResponse, originalBodyStream);
            }
            else
            {
                // 🔴 Localized fallback for error messages
                var message = _localizer["GenericError"];
                object errorPayload = new { message };

                try
                {
                    if (!string.IsNullOrEmpty(responseContent))
                    {
                        var errorObj = JsonSerializer.Deserialize<JsonElement>(responseContent);

                        if (errorObj.TryGetProperty("title", out var title))
                            message = new LocalizedString(nameof(title), title.GetString() ?? message.Value);
                        else if (errorObj.TryGetProperty("detail", out var detail))
                            message = new LocalizedString(nameof(detail), detail.GetString() ?? message.Value);

                        errorPayload = new { message = message.Value };
                    }

                }
                catch
                {
                    // fallback
                }

                var apiErrorResponse = new ApiErrorResponse(context.Response.StatusCode, errorPayload, apiVersion);
                await WriteJsonResponse(context, apiErrorResponse, originalBodyStream);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception, Stream originalBodyStream)
        {
            context.Response.Body = originalBodyStream;
            context.Response.ContentType = "application/json";

            var apiVersion = GetApiVersionFromRequest(context);

            var localizedMessage = exception switch
            {
                UnauthorizedAccessException => _localizer["Unauthorized"],
                KeyNotFoundException => _localizer["NotFound"],
                _ => _localizer["InternalServerError"]
            };

            var apiError = new ApiErrorResponse(
                context.Response.StatusCode == 0 ? 500 : context.Response.StatusCode,
                new { message = localizedMessage },
                apiVersion
            );

            await WriteJsonResponse(context, apiError, originalBodyStream);
        }

        private async Task WriteJsonResponse(HttpContext context, object response, Stream originalBodyStream)
        {
            context.Response.Body = originalBodyStream;
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 200;
            context.Response.Headers.Remove("Content-Length");

            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = context.RequestServices.GetService<IWebHostEnvironment>()?.IsDevelopment() == true
            });

            await context.Response.WriteAsync(jsonResponse);
        }

        private static string GetApiVersionFromRequest(HttpContext context)
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

    // --------------------- Action Filter ---------------------

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

    // --------------------- Response Service ---------------------

    public interface IApiResponseService
    {
        IActionResult FormatResponse(object? data, ActionContext context);
    }

    public class ApiResponseService : IApiResponseService
    {
        private readonly IStringLocalizer<SharedResource> _localizer;

        public ApiResponseService(IStringLocalizer<SharedResource> localizer)
        {
            _localizer = localizer;
        }

        public IActionResult FormatResponse(object? data, ActionContext context)
        {
            var statusCode = context.HttpContext.Response.StatusCode;
            var success = statusCode >= 200 && statusCode < 300;
            var apiVersion = GetApiVersionFromRequest(context.HttpContext);

            if (success)
                return new ObjectResult(ApiResponse.SuccessResponse(data, _localizer["RequestSuccessful"], statusCode, apiVersion)) { StatusCode = 200 };

            var errorKey = statusCode switch
            {
                400 => "BadRequest",
                401 => "Unauthorized",
                403 => "Forbidden",
                404 => "NotFound",
                500 => "InternalServerError",
                _ => "GenericError"
            };

            return new ObjectResult(new ApiErrorResponse(statusCode, new { message = _localizer[errorKey] }, apiVersion)) { StatusCode = 200 };
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
