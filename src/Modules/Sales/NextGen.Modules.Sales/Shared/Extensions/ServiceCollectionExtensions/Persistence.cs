using BuildingBlocks.Abstractions.Persistence;
using BuildingBlocks.Persistence.EfCore.SqlServer;
using BuildingBlocks.Persistence.Mongo;
using NextGen.Modules.Sales.Shared.Contracts;
using NextGen.Modules.Sales.Shared.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace NextGen.Modules.Sales.Shared.Extensions.ServiceCollectionExtensions;

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
                $"{SalesModuleConfiguration.ModuleName}:{nameof(SqlServerOptions)}:UseInMemory"))
        {
            services.AddDbContext<SalesDbContext>(options =>
                options.UseInMemoryDatabase("NextGen.Modules.Parties"));

            services.AddScoped<IDbFacadeResolver>(provider => provider.GetService<SalesDbContext>()!);
        }
        else
        {
            services.AddSqlServerDbContext<SalesDbContext>(
                configuration,
                $"{SalesModuleConfiguration.ModuleName}:{nameof(SqlServerOptions)}");
        }

        services.AddScoped<ISalesDbContext>(provider => provider.GetRequiredService<SalesDbContext>());
    }

    private static void AddMongoReadStorage(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMongoDbContext<SaleReadDbContext>(
            configuration,
            $"{SalesModuleConfiguration.ModuleName}:{nameof(MongoOptions)}");
    }
}
