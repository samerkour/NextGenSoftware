using BuildingBlocks.Abstractions.Persistence;
using BuildingBlocks.Persistence.EfCore.SqlServer;
using BuildingBlocks.Persistence.Mongo;
using NextGen.Modules.Inventories.Shared.Contracts;
using NextGen.Modules.Inventories.Shared.Data;
using Microsoft.EntityFrameworkCore;

namespace NextGen.Modules.Inventories.Shared.Extensions.ServiceCollectionExtensions;

public static partial class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddStorage(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        AddStorage(builder.Services, configuration);

        return builder;
    }

    public static IServiceCollection AddStorage(this IServiceCollection services, IConfiguration configuration)
    {
        AddSqlServerWriteStorage(services, configuration);
        AddMongoReadStorage(services, configuration);

        return services;
    }

    private static void AddSqlServerWriteStorage(IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.GetValue<bool>(
                $"{InventoryModuleConfiguration.ModuleName}:{nameof(SqlServerOptions)}:UseInMemory"))
        {
            services.AddDbContext<InventoryDbContext>(options =>
                options.UseInMemoryDatabase("NextGen.Modules.Inventories"));

            services.AddScoped<IDbFacadeResolver>(provider => provider.GetService<InventoryDbContext>()!);
        }
        else
        {
            services.AddSqlServerDbContext<InventoryDbContext>(
                configuration,
                $"{InventoryModuleConfiguration.ModuleName}:{nameof(SqlServerOptions)}");
        }

        services.AddScoped<IInventoryDbContext>(provider => provider.GetRequiredService<InventoryDbContext>());
    }

    private static void AddMongoReadStorage(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMongoDbContext<InventoryReadDbContext>(
            configuration,
            $"{InventoryModuleConfiguration.ModuleName}:{nameof(MongoOptions)}");
    }
}
