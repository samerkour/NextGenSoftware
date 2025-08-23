using Asp.Versioning.Builder;
using BuildingBlocks.Abstractions.CQRS.Event;
using BuildingBlocks.Abstractions.Persistence;
using NextGen.Modules.Catalogs.Products.Data;
using NextGen.Modules.Catalogs.Products.Features.CreatingProduct;
using NextGen.Modules.Catalogs.Products.Features.DebitingProductStock;
using NextGen.Modules.Catalogs.Products.Features.GettingProductById;
using NextGen.Modules.Catalogs.Products.Features.GettingProductsView;
using NextGen.Modules.Catalogs.Products.Features.ReplenishingProductStock;

namespace NextGen.Modules.Catalogs.Products;

internal static class ProductsConfigs
{
    public const string Tag = "Product";
    public const string ProductsPrefixUri = $"{CatalogModuleConfiguration.CatalogModulePrefixUri}/products";
    public static ApiVersionSet VersionSet { get; private set; } = default!;

    internal static IServiceCollection AddProductsServices(this IServiceCollection services)
    {
        services.AddScoped<IDataSeeder, ProductDataSeeder>();
        services.AddSingleton<IEventMapper, ProductEventMapper>();

        return services;
    }

    internal static IEndpointRouteBuilder MapProductsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        VersionSet = endpoints.NewApiVersionSet(Tag).Build();

        return endpoints.MapCreateProductsEndpoint()
            .MapDebitProductStockEndpoint()
            .MapReplenishProductStockEndpoint()
            .MapGetProductByIdEndpoint()
            .MapGetProductsViewEndpoint();
    }
}
