using Ardalis.GuardClauses;
using BuildingBlocks.Core.Extensions;
using BuildingBlocks.Monitoring;
using BuildingBlocks.Persistence.EfCore.SqlServer;
using NextGen.Modules.Catalogs;
using NextGen.Modules.Customers;
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
            var catalogSqlServerOptions = configuration.GetOptions<SqlServerOptions>(
                $"{CatalogModuleConfiguration.ModuleName}:{nameof(SqlServerOptions)}");

            Guard.Against.Null(catalogSqlServerOptions, nameof(catalogSqlServerOptions));

            healthChecksBuilder.AddSqlServer(
                catalogSqlServerOptions.ConnectionString,
                name: "Catalogs-Module-SqlServer-Check",
                tags: new[] {"catalogs-sqlserver"});


            var customerSqlServerOptions = configuration.GetOptions<SqlServerOptions>(
                $"{CustomersModuleConfiguration.ModuleName}:{nameof(SqlServerOptions)}");

            Guard.Against.Null(customerSqlServerOptions, nameof(customerSqlServerOptions));

            healthChecksBuilder.AddSqlServer(
                customerSqlServerOptions.ConnectionString,
                name: "Customers-Module-SqlServer-Check",
                tags: new[] {"customers-sqlserver"});

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
