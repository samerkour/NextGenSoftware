using BuildingBlocks.Persistence.EfCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using NextGen.Modules.Inventories.Shared.Data;

namespace NextGen.Modules.Inventories.Shared.Extensions.ApplicationBuilderExtensions;

public static partial class ApplicationBuilderExtensions
{
    public static async Task ApplyDatabaseMigrations(this IApplicationBuilder app, ILogger logger)
    {
        var configuration = app.ApplicationServices.GetRequiredService<IConfiguration>();
        if (!configuration.GetValue<bool>(
                $"{InventoryModuleConfiguration.ModuleName}:{nameof(SqlServerOptions)}:UseInMemory"))
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var inventoryDbContext = serviceScope.ServiceProvider.GetRequiredService<InventoryDbContext>();

            logger.LogInformation("Updating inventory database...");

            await inventoryDbContext.Database.MigrateAsync();

            logger.LogInformation("Updated inventory database");
        }
    }
}
