using BuildingBlocks.Abstractions.Persistence;
using NextGen.Modules.Inventories.Categories.Data;

namespace NextGen.Modules.Inventories.Categories;

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
