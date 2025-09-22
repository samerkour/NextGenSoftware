using Asp.Versioning;
using Asp.Versioning.Builder;
using BuildingBlocks.Abstractions.Web.Module;
using BuildingBlocks.Core;
using BuildingBlocks.Core.Extensions;
using BuildingBlocks.Core.Messaging.Extensions;
using BuildingBlocks.Web.Extensions;
using NextGen.Modules.Inventories.Brands;
using NextGen.Modules.Inventories.Categories;
using NextGen.Modules.Inventories.Products;
using NextGen.Modules.Inventories.Shared.Extensions.ApplicationBuilderExtensions;
using NextGen.Modules.Inventories.Shared.Extensions.ServiceCollectionExtensions;
using NextGen.Modules.Inventories.Suppliers;

namespace NextGen.Modules.Inventories;

public class InventoryModuleConfiguration : IModuleDefinition
{
    public const string InventoryModulePrefixUri = "api/v{version:apiVersion}/inventories";
    public const string ModuleName = "Inventories";
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

        endpoints.MapGet("inventories", (HttpContext context) =>
        {
            var requestId = context.Request.Headers.TryGetValue("X-Request-Id", out var requestIdHeader)
                ? requestIdHeader.FirstOrDefault()
                : string.Empty;

            return $"Inventories Service Apis, RequestId: {requestId}";
        }).ExcludeFromDescription();
    }
}
