using BuildingBlocks.Abstractions.Persistence;
using NextGen.Modules.Inventorys.Brands.Data;

namespace NextGen.Modules.Inventorys.Brands;

internal static class Configs
{
    internal static IServiceCollection AddBrandsServices(this IServiceCollection services)
    {
        services.AddScoped<IDataSeeder, BrandDataSeeder>();

        return services;
    }

    internal static IEndpointRouteBuilder MapBrandsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        return endpoints;
    }
}
