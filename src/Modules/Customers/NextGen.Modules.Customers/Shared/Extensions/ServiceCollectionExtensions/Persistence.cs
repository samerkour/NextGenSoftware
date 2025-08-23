using BuildingBlocks.Abstractions.Persistence;
using BuildingBlocks.Persistence.EfCore.SqlServer;
using BuildingBlocks.Persistence.Mongo;
using NextGen.Modules.Customers.Shared.Contracts;
using NextGen.Modules.Customers.Shared.Data;
using Microsoft.EntityFrameworkCore;

namespace NextGen.Modules.Customers.Shared.Extensions.ServiceCollectionExtensions;

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
                $"{CustomersModuleConfiguration.ModuleName}:{nameof(SqlServerOptions)}:UseInMemory"))
        {
            services.AddDbContext<CustomersDbContext>(options =>
                options.UseInMemoryDatabase("NextGen.Modules.Customers"));

            services.AddScoped<IDbFacadeResolver>(provider => provider.GetService<CustomersDbContext>()!);
        }
        else
        {
            services.AddSqlServerDbContext<CustomersDbContext>(
                configuration,
                $"{CustomersModuleConfiguration.ModuleName}:{nameof(SqlServerOptions)}");
        }

        services.AddScoped<ICustomersDbContext>(provider => provider.GetRequiredService<CustomersDbContext>());
    }

    private static void AddMongoReadStorage(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMongoDbContext<CustomersReadDbContext>(
            configuration,
            $"{CustomersModuleConfiguration.ModuleName}:{nameof(MongoOptions)}");
    }
}
