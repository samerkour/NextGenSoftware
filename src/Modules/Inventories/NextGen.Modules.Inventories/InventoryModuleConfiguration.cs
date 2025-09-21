using Asp.Versioning;
using Asp.Versioning.Builder;
using BuildingBlocks.Abstractions.Web.Module;
using BuildingBlocks.Core;
using BuildingBlocks.Core.Extensions;
using BuildingBlocks.Core.Messaging.Extensions;
using BuildingBlocks.Web.Extensions;
using NextGen.Modules.Inventorys.Brands;
using NextGen.Modules.Inventorys.Categories;
using NextGen.Modules.Inventorys.Products;
using NextGen.Modules.Inventorys.Shared.Extensions.ApplicationBuilderExtensions;
using NextGen.Modules.Inventorys.Shared.Extensions.ServiceCollectionExtensions;
using NextGen.Modules.Inventorys.Suppliers;

namespace NextGen.Modules.Inventorys;

public class InventoryModuleConfiguration : IModuleDefinition
{
    public const string InventoryModulePrefixUri = "api/v{version:apiVersion}/inventorys";
    public const string ModuleName = "Inventorys";
    public void AddModuleServices(
        IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        services.AddInfrastructure(configuration);
        services.AddStorage(configuration);

        // Add Sub Modules Services
        services.AddBrandsServices();
        services.AddCategoriesServices();
        services.AddSuppliersServices();
        services.AddProductsServices();
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

        app.SubscribeAllMessageFromAssemblyOfType<InventoryRoot>();

        app.UseInfrastructure();

        await app.ApplyDatabaseMigrations(logger);
        await app.SeedData(logger, environment);
    }

    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        // Add Sub Modules Endpoints
        endpoints.MapProductsEndpoints();

        endpoints.MapGet("inventorys", (HttpContext context) =>
        {
            var requestId = context.Request.Headers.TryGetValue("X-Request-Id", out var requestIdHeader)
                ? requestIdHeader.FirstOrDefault()
                : string.Empty;

            return $"Inventorys Service Apis, RequestId: {requestId}";
        }).ExcludeFromDescription();
    }
}
