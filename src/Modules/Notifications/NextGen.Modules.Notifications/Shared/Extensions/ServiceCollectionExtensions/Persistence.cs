using BuildingBlocks.Abstractions.Persistence;
using BuildingBlocks.Persistence.EfCore.SqlServer;
using BuildingBlocks.Persistence.Mongo;
using NextGen.Modules.Notifications.Shared.Contracts;
using NextGen.Modules.Notifications.Shared.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace NextGen.Modules.Notifications.Shared.Extensions.ServiceCollectionExtensions;

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
                $"{NotificationsModuleConfiguration.ModuleName}:{nameof(SqlServerOptions)}:UseInMemory"))
        {
            services.AddDbContext<NotificationsDbContext>(options =>
                options.UseInMemoryDatabase("NextGen.Modules.Parties"));

            services.AddScoped<IDbFacadeResolver>(provider => provider.GetService<NotificationsDbContext>()!);
        }
        else
        {
            services.AddSqlServerDbContext<NotificationsDbContext>(
                configuration,
                $"{NotificationsModuleConfiguration.ModuleName}:{nameof(SqlServerOptions)}");
        }

        services.AddScoped<INotificationsDbContext>(provider => provider.GetRequiredService<NotificationsDbContext>());
    }

    private static void AddMongoReadStorage(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMongoDbContext<NotificationReadDbContext>(
            configuration,
            $"{NotificationsModuleConfiguration.ModuleName}:{nameof(MongoOptions)}");
    }
}
