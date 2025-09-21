using Ardalis.GuardClauses;
using BuildingBlocks.Core.Extensions;
using BuildingBlocks.Monitoring;
using BuildingBlocks.Persistence.EfCore.SqlServer;
using NextGen.Modules.Inventorys;
using NextGen.Modules.Parties;
using NextGen.Modules.Identity;
using NextGen.Modules.Orders;

namespace NextGen.Api.Extensions.ServiceCollectionExtensions;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddNextGenMonitoring(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMonitoring(healthChecksBuilder =>
        {
            var inventorySqlServerOptions = configuration.GetOptions<SqlServerOptions>(
                $"{InventoryModuleConfiguration.ModuleName}:{nameof(SqlServerOptions)}");

            Guard.Against.Null(inventorySqlServerOptions, nameof(inventorySqlServerOptions));

            healthChecksBuilder.AddSqlServer(
                inventorySqlServerOptions.ConnectionString,
                name: "Inventorys-Module-SqlServer-Check",
                tags: new[] {"inventorys-sqlserver"});


            var partySqlServerOptions = configuration.GetOptions<SqlServerOptions>(
                $"{PartiesModuleConfiguration.ModuleName}:{nameof(SqlServerOptions)}");

            Guard.Against.Null(partySqlServerOptions, nameof(partySqlServerOptions));

            healthChecksBuilder.AddSqlServer(
                partySqlServerOptions.ConnectionString,
                name: "Parties-Module-SqlServer-Check",
                tags: new[] {"parties-sqlserver"});

            var identitySqlServerOptions = configuration.GetOptions<SqlServerOptions>(
                $"{IdentityModuleConfiguration.ModuleName}:{nameof(SqlServerOptions)}");
            Guard.Against.Null(identitySqlServerOptions, nameof(identitySqlServerOptions));

            healthChecksBuilder.AddSqlServer(
                identitySqlServerOptions.ConnectionString,
                name: "Identity-Module-SqlServer-Check",
                tags: new[] {"identity-sqlserver"});

            var orderSqlServerOptions =
                configuration.GetOptions<SqlServerOptions>(
                    $"{OrdersModuleConfiguration.ModuleName}:{nameof(SqlServerOptions)}");

            Guard.Against.Null(orderSqlServerOptions, nameof(orderSqlServerOptions));

            healthChecksBuilder.AddSqlServer(
                orderSqlServerOptions.ConnectionString,
                name: "Orders-Modules-SqlServer-Check",
                tags: new[] {"orders-sqlserver"});
        });

        return services;
    }
}
