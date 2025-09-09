using BuildingBlocks.Persistence.EfCore.SqlServer;
using NextGen.Modules.Catalogs.Shared.Data;
using Microsoft.EntityFrameworkCore;

namespace NextGen.Modules.Catalogs.Shared.Extensions.ApplicationBuilderExtensions;

public static partial class ApplicationBuilderExtensions
{
    public static async Task ApplyDatabaseMigrations(this IApplicationBuilder app, ILogger logger)
    {
        var configuration = app.ApplicationServices.GetRequiredService<IConfiguration>();
        if (!configuration.GetValue<bool>(
                $"{CatalogModuleConfiguration.ModuleName}:{nameof(SqlServerOptions)}:UseInMemory"))
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var catalogDbContext = serviceScope.ServiceProvider.GetRequiredService<CatalogDbContext>();

            logger.LogInformation("Updating catalog database...");

            var con = catalogDbContext.Database.GetConnectionString();
            var con1= catalogDbContext.Database.GetDbConnection().ConnectionString;
            await catalogDbContext.Database.MigrateAsync();

            logger.LogInformation("Updated catalog database");
        }
    }
}
