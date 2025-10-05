using Ardalis.GuardClauses;
using BuildingBlocks.Core.Extensions;
using BuildingBlocks.Monitoring;
using BuildingBlocks.Persistence.EfCore.SqlServer;
using NextGen.Modules.Inventories;
using NextGen.Modules.Parties;
using NextGen.Modules.Identity;
using NextGen.Modules.Sales;

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
                name: "Inventories-Module-SqlServer-Check",
                tags: new[] {"inventories-sqlserver"});


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

            var saleSqlServerOptions =
                configuration.GetOptions<SqlServerOptions>(
                    $"{SalesModuleConfiguration.ModuleName}:{nameof(SqlServerOptions)}");

            Guard.Against.Null(saleSqlServerOptions, nameof(saleSqlServerOptions));

            healthChecksBuilder.AddSqlServer(
                saleSqlServerOptions.ConnectionString,
                name: "Sales-Modules-SqlServer-Check",
                tags: new[] {"sales-sqlserver"});
        });

        return services;
    }
}
