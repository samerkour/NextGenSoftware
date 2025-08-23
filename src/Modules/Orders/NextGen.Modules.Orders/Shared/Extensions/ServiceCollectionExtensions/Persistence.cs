using BuildingBlocks.Abstractions.Persistence;
using BuildingBlocks.Persistence.EfCore.SqlServer;
using BuildingBlocks.Persistence.Mongo;
using NextGen.Modules.Orders.Shared.Contracts;
using NextGen.Modules.Orders.Shared.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace NextGen.Modules.Orders.Shared.Extensions.ServiceCollectionExtensions;

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
                $"{OrdersModuleConfiguration.ModuleName}:{nameof(SqlServerOptions)}:UseInMemory"))
        {
            services.AddDbContext<OrdersDbContext>(options =>
                options.UseInMemoryDatabase("NextGen.Modules.Customers"));

            services.AddScoped<IDbFacadeResolver>(provider => provider.GetService<OrdersDbContext>()!);
        }
        else
        {
            services.AddSqlServerDbContext<OrdersDbContext>(
                configuration,
                $"{OrdersModuleConfiguration.ModuleName}:{nameof(SqlServerOptions)}");
        }

        services.AddScoped<IOrdersDbContext>(provider => provider.GetRequiredService<OrdersDbContext>());
    }

    private static void AddMongoReadStorage(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMongoDbContext<OrderReadDbContext>(
            configuration,
            $"{OrdersModuleConfiguration.ModuleName}:{nameof(MongoOptions)}");
    }
}
