using BuildingBlocks.Abstractions.Persistence;
using NextGen.Modules.Inventories.Suppliers.Data;

namespace NextGen.Modules.Inventories.Suppliers;

internal static class Configs
{
    internal static IServiceCollection AddSuppliersServices(this IServiceCollection services)
    {
        services.AddScoped<IDataSeeder, SupplierDataSeeder>();

        return services;
    }

    internal static IEndpointRouteBuilder MapSuppliersEndpoints(this IEndpointRouteBuilder endpoints)
    {
        return endpoints;
    }
}
