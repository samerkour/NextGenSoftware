using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.Scheduling;
using BuildingBlocks.Core.Extensions;
using BuildingBlocks.Core.Extensions.ServiceCollection;
using Hangfire;
using Hangfire.InMemory;
using Hangfire.SqlServer;
using Microsoft.Extensions.Configuration;

namespace BuildingBlocks.Integration.Hangfire;

public static class Extensions
{
    public static IServiceCollection AddCustomHangfire(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<IGlobalConfiguration>? configurator = null)
    {
        var hangfireOptions = configuration.GetOptions<HangfireOptions>(nameof(HangfireOptions));

        Guard.Against.Null(hangfireOptions, nameof(hangfireOptions));

        services.AddHangfire(cfg =>
        {
            cfg.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings();

            if (!hangfireOptions.UseInMemory)
            {
                cfg.UseSqlServerStorage(
                    hangfireOptions.ConnectionString,
                    new SqlServerStorageOptions
                    {
                        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),  // Added recommended setting
                        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),  // Fixed property name
                        QueuePollInterval = TimeSpan.Zero,
                        UseRecommendedIsolationLevel = true,  // Added for better performance
                        PrepareSchemaIfNecessary = true // Ensures tables are created automatically
                    });
            }
            else
            {
                cfg.UseInMemoryStorage();
            }

            configurator?.Invoke(cfg);
        });

        // Add the processing server as IHostedService
        services.AddHangfireServer();

        services.AddTransient<IScheduleExecutor, MediatRExecutor>();
        services.ReplaceTransient<IScheduler, HangfireScheduler>();

        return services;
    }

    public static IServiceCollection AddHangfireCommandScheduler(
        this IServiceCollection services,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
    {
        services.Replace<ICommandScheduler, HangfireCommandScheduler>(serviceLifetime);

        return services;
    }
}
