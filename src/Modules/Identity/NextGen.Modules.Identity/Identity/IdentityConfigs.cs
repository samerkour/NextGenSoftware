using Asp.Versioning.Builder;
using BuildingBlocks.Abstractions.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using NextGen.Modules.Identity.Identity.Data;
using NextGen.Modules.Identity.Identity.Features.ForgotPassword;
using NextGen.Modules.Identity.Identity.Features.GenerateCaptcha;
using NextGen.Modules.Identity.Identity.Features.GetClaims;
using NextGen.Modules.Identity.Identity.Features.Login;
using NextGen.Modules.Identity.Identity.Features.Logout;
using NextGen.Modules.Identity.Identity.Features.RefreshingToken;
using NextGen.Modules.Identity.Identity.Features.ResetPassword;
using NextGen.Modules.Identity.Identity.Features.Logout;
using NextGen.Modules.Identity.Identity.Features.SendEmailVerificationCode;
using NextGen.Modules.Identity.Identity.Features.VerifyEmail;
using NextGen.Modules.Identity.Shared.Extensions.ServiceCollectionExtensions;

namespace NextGen.Modules.Identity.Identity;
internal static class IdentityConfigs
{
    public const string Tag = "authentication";
    public const string IdentityPrefixUri = $"{IdentityModuleConfiguration.IdentityModulePrefixUri}/{Tag}";
    public static ApiVersionSet VersionSet { get; private set; } = default!;

    internal static IServiceCollection AddIdentityServices(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        services.AddCustomIdentity(
            configuration,
            $"{IdentityModuleConfiguration.ModuleName}:{nameof(IdentityOptions)}");

        services.AddScoped<IDataSeeder, IdentityDataSeeder>();

        if (environment.IsEnvironment("test") == false)
            services.AddCustomIdentityServer();

        return services;
    }

    internal static IEndpointRouteBuilder MapIdentityEndpoints(this IEndpointRouteBuilder endpoints)
    {
        VersionSet = endpoints.NewApiVersionSet(Tag).Build();

        endpoints.MapGet(
            $"{IdentityPrefixUri}/user-role",
            [Authorize(
                AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
                Roles = Constants.Role.User)]
            () => new {Role = Constants.Role.User}).WithTags("Identity");

        endpoints.MapGet(
            $"{IdentityPrefixUri}/admin-role",
            [Authorize(
                AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
                Roles = Constants.Role.Admin)]
            () => new {Role = Constants.Role.Admin}).WithTags("Identity");

        endpoints.MapGenerateCaptchaEndpoint();
        endpoints.MapLoginUserEndpoint();
        endpoints.MapResetPasswordEndpoint();
        endpoints.MapForgotPasswordEndpoint();
        endpoints.MapSendEmailVerificationCodeEndpoint();
        endpoints.MapSendVerifyEmailEndpoint();
        endpoints.MapRefreshTokenEndpoint();
        endpoints.MapRevokeTokenEndpoint();
        endpoints.MapGetClaimsEndpoint();

        return endpoints;
    }
}
