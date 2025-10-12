using Asp.Versioning;
using Asp.Versioning.Builder;
using BuildingBlocks.Abstractions.Web.Module;
using BuildingBlocks.Core.Extensions;
using BuildingBlocks.Core.Messaging.Extensions;
using NextGen.Modules.Identity.ClaimGroups;
using NextGen.Modules.Identity.Claims;
using NextGen.Modules.Identity.Identity;
using NextGen.Modules.Identity.Shared.Extensions.ApplicationBuilderExtensions;
using NextGen.Modules.Identity.Shared.Extensions.ServiceCollectionExtensions;
using NextGen.Modules.Identity.Users;

namespace NextGen.Modules.Identity;

public class IdentityModuleConfiguration : IModuleDefinition
{
    public const string IdentityModulePrefixUri = "api/v{version:apiVersion}";
    public const string ModuleName = "Identity";
    public static ApiVersionSet VersionSet { get; private set; } = default!;

    public void AddModuleServices(
        IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        services.AddInfrastructure(configuration);

        // Add Sub Modules Endpoints
        services.AddIdentityServices(configuration, environment);
        services.AddUsersServices(configuration);
        services.AddClaimsServices(configuration);
        services.AddClaimGroupServices(configuration);
    }

    public async Task ConfigureModule(
        IApplicationBuilder app,
        IConfiguration configuration,
        ILogger logger,
        IWebHostEnvironment environment)
    {
        if (environment.IsEnvironment("test") == false)
        {
            // HostedServices just run on main service provider - It should not await because it will block the main thread.
            await app.ApplicationServices.StartHostedServices();
            app.UseIdentityServer();

            // TODO: Add Monitoring
        }

        app.SubscribeAllMessageFromAssemblyOfType<IdentityRoot>();

        await app.ApplyDatabaseMigrations(logger);
        await app.SeedData(logger, environment);
    }

    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var v1 = new ApiVersion(1, 0);
        var v2 = new ApiVersion(2, 0);
        var v3 = new ApiVersion(3, 0);

        VersionSet = endpoints.NewApiVersionSet()
            .HasApiVersion(v1)
            .HasApiVersion(v2)
            .HasApiVersion(v3)
            .Build();

        endpoints.MapGet("identity", (HttpContext context) =>
        {
            var requestId = context.Request.Headers.TryGetValue("X-Request-Id", out var requestIdHeader)
                ? requestIdHeader.FirstOrDefault()
                : string.Empty;

            return $"Identity Service Apis, RequestId: {requestId}";
        }).ExcludeFromDescription();

        // Add Sub Modules Endpoints
        endpoints.MapIdentityEndpoints();
        endpoints.MapUsersEndpoints();
        endpoints.MapClaimEndpoints();
        endpoints.MapClaimGroupEndpoints();
    }
}
