using BuildingBlocks.Abstractions.Persistence;
using NextGen.Modules.Catalogs.Categories.Data;

namespace NextGen.Modules.Catalogs.Categories;

internal static class Configs
{
    internal static IServiceCollection AddCategoriesServices(this IServiceCollection services)
    {
        services.AddScoped<IDataSeeder, CategoryDataSeeder>();

        return services;
    }

    internal static IEndpointRouteBuilder MapCategoriesEndpoints(this IEndpointRouteBuilder endpoints)
    {

        return endpoints;
    }
}
