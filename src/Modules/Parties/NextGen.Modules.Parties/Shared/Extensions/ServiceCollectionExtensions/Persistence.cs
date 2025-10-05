using BuildingBlocks.Abstractions.Persistence;
using BuildingBlocks.Persistence.EfCore.SqlServer;
using BuildingBlocks.Persistence.Mongo;
using NextGen.Modules.Parties.Shared.Contracts;
using NextGen.Modules.Parties.Shared.Data;
using Microsoft.EntityFrameworkCore;

namespace NextGen.Modules.Parties.Shared.Extensions.ServiceCollectionExtensions;

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
                $"{PartiesModuleConfiguration.ModuleName}:{nameof(SqlServerOptions)}:UseInMemory"))
        {
            services.AddDbContext<PartiesDbContext>(options =>
                options.UseInMemoryDatabase("NextGen.Modules.Parties"));

            services.AddScoped<IDbFacadeResolver>(provider => provider.GetService<PartiesDbContext>()!);
        }
        else
        {
            services.AddSqlServerDbContext<PartiesDbContext>(
                configuration,
                $"{PartiesModuleConfiguration.ModuleName}:{nameof(SqlServerOptions)}");
        }

        services.AddScoped<IPartiesDbContext>(provider => provider.GetRequiredService<PartiesDbContext>());
    }

    private static void AddMongoReadStorage(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMongoDbContext<PartiesReadDbContext>(
            configuration,
            $"{PartiesModuleConfiguration.ModuleName}:{nameof(MongoOptions)}");
    }
}
