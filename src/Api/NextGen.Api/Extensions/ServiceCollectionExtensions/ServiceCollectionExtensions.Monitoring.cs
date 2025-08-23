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
    public static IServiceCollection AddFoodDeliveryMonitoring(
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
                name: "Catalogs-Module-Postgres-Check",
                tags: new[] {"catalogs-postgres"});


            var customerSqlServerOptions = configuration.GetOptions<SqlServerOptions>(
                $"{CustomersModuleConfiguration.ModuleName}:{nameof(SqlServerOptions)}");

            Guard.Against.Null(customerSqlServerOptions, nameof(customerSqlServerOptions));

            healthChecksBuilder.AddSqlServer(
                customerSqlServerOptions.ConnectionString,
                name: "Customers-Module-Postgres-Check",
                tags: new[] {"customers-postgres"});

            var identitySqlServerOptions = configuration.GetOptions<SqlServerOptions>(
                $"{IdentityModuleConfiguration.ModuleName}:{nameof(SqlServerOptions)}");
            Guard.Against.Null(identitySqlServerOptions, nameof(identitySqlServerOptions));

            healthChecksBuilder.AddSqlServer(
                identitySqlServerOptions.ConnectionString,
                name: "Identity-Module-Postgres-Check",
                tags: new[] {"identity-postgres"});

            var orderSqlServerOptions =
                configuration.GetOptions<SqlServerOptions>(
                    $"{OrdersModuleConfiguration.ModuleName}:{nameof(SqlServerOptions)}");

            Guard.Against.Null(orderSqlServerOptions, nameof(orderSqlServerOptions));

            healthChecksBuilder.AddSqlServer(
                orderSqlServerOptions.ConnectionString,
                name: "Orders-Modules-Postgres-Check",
                tags: new[] {"orders-postgres"});
        });

        return services;
    }
}
