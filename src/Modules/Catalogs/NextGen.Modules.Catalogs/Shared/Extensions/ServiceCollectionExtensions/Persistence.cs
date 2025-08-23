using BuildingBlocks.Abstractions.Persistence;
using BuildingBlocks.Persistence.EfCore.SqlServer;
using BuildingBlocks.Persistence.Mongo;
using NextGen.Modules.Catalogs.Shared.Contracts;
using NextGen.Modules.Catalogs.Shared.Data;
using Microsoft.EntityFrameworkCore;

namespace NextGen.Modules.Catalogs.Shared.Extensions.ServiceCollectionExtensions;

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
                $"{CatalogModuleConfiguration.ModuleName}:{nameof(SqlServerOptions)}:UseInMemory"))
        {
            services.AddDbContext<CatalogDbContext>(options =>
                options.UseInMemoryDatabase("NextGen.Modules.Catalogs"));

            services.AddScoped<IDbFacadeResolver>(provider => provider.GetService<CatalogDbContext>()!);
        }
        else
        {
            services.AddSqlServerDbContext<CatalogDbContext>(
                configuration,
                $"{CatalogModuleConfiguration.ModuleName}:{nameof(SqlServerOptions)}");
        }

        services.AddScoped<ICatalogDbContext>(provider => provider.GetRequiredService<CatalogDbContext>());
    }

    private static void AddMongoReadStorage(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMongoDbContext<CatalogReadDbContext>(
            configuration,
            $"{CatalogModuleConfiguration.ModuleName}:{nameof(MongoOptions)}");
    }
}
