using BuildingBlocks.Persistence.EfCore.SqlServer;
using NextGen.Modules.Sales.Shared.Data;
using Microsoft.EntityFrameworkCore;

namespace NextGen.Modules.Sales.Shared.Extensions.ApplicationBuilderExtensions;

public static partial class ApplicationBuilderExtensions
{
    public static async Task ApplyDatabaseMigrations(this IApplicationBuilder app, ILogger logger)
    {
        var configuration = app.ApplicationServices.GetRequiredService<IConfiguration>();

        if (!configuration.GetValue<bool>(
                $"{SalesModuleConfiguration.ModuleName}:{nameof(SqlServerOptions)}:UseInMemory"))
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var dbContext = serviceScope.ServiceProvider.GetRequiredService<SalesDbContext>();

            logger.LogInformation("Updating inventory database...");

            await dbContext.Database.MigrateAsync();

            logger.LogInformation("Updated inventory database");
        }
    }
}
