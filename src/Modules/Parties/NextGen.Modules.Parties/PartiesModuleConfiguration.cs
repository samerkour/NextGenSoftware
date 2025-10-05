using Asp.Versioning;
using Asp.Versioning.Builder;
using BuildingBlocks.Abstractions.Web.Module;
using BuildingBlocks.Core;
using BuildingBlocks.Core.Extensions;
using BuildingBlocks.Core.Messaging.Extensions;
using NextGen.Modules.Parties.Parties;
using NextGen.Modules.Parties.RestockSubscriptions;
using NextGen.Modules.Parties.Shared.Extensions.ApplicationBuilderExtensions;
using NextGen.Modules.Parties.Shared.Extensions.ServiceCollectionExtensions;

namespace NextGen.Modules.Parties;

public class PartiesModuleConfiguration : IModuleDefinition
{
    public const string PartyModulePrefixUri = "api/v{version:apiVersion}/parties";
    public const string ModuleName = "Parties";

    public void AddModuleServices(
        IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        services.AddInfrastructure(configuration);

        services.AddStorage(configuration);

        // Add Sub Modules Services
        services.AddPartiesServices();
        services.AddRestockSubscriptionServices();
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
        }

        ServiceActivator.Configure(app.ApplicationServices);

        app.SubscribeAllMessageFromAssemblyOfType<PartiesRoot>();

        await app.ApplyDatabaseMigrations(logger);
        await app.SeedData(logger, environment);
    }

    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        // Add Sub Modules Endpoints
        endpoints.MapPartiesEndpoints();
        endpoints.MapRestockSubscriptionsEndpoints();

        endpoints.MapGet("parties", (HttpContext context) =>
        {
            var requestId = context.Request.Headers.TryGetValue("X-Request-Id", out var requestIdHeader)
                ? requestIdHeader.FirstOrDefault()
                : string.Empty;

            return $"Parties Service Apis, RequestId: {requestId}";
        }).ExcludeFromDescription();
    }
}
