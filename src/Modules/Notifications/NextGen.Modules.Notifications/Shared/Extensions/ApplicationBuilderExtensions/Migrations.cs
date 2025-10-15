using BuildingBlocks.Persistence.EfCore.SqlServer;
using NextGen.Modules.Notifications.Shared.Data;
using Microsoft.EntityFrameworkCore;

namespace NextGen.Modules.Notifications.Shared.Extensions.ApplicationBuilderExtensions;

public static partial class ApplicationBuilderExtensions
{
    public static async Task ApplyDatabaseMigrations(this IApplicationBuilder app, ILogger logger)
    {
        var configuration = app.ApplicationServices.GetRequiredService<IConfiguration>();

        if (!configuration.GetValue<bool>(
                $"{NotificationsModuleConfiguration.ModuleName}:{nameof(SqlServerOptions)}:UseInMemory"))
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var dbContext = serviceScope.ServiceProvider.GetRequiredService<NotificationsDbContext>();

            logger.LogInformation("Updating inventory database...");

            await dbContext.Database.MigrateAsync();

            logger.LogInformation("Updated inventory database");
        }
    }
}
