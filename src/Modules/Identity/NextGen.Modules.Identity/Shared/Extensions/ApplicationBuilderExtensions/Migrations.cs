using BuildingBlocks.Persistence.EfCore.SqlServer;
using NextGen.Modules.Identity.Shared.Data;
using Microsoft.EntityFrameworkCore;

namespace NextGen.Modules.Identity.Shared.Extensions.ApplicationBuilderExtensions;

public static partial class ApplicationBuilderExtensions
{
    public static async Task ApplyDatabaseMigrations(this IApplicationBuilder app, ILogger logger)
    {
        var configuration = app.ApplicationServices.GetRequiredService<IConfiguration>();
        if (!configuration.GetValue<bool>($"{IdentityModuleConfiguration.ModuleName}:{nameof(SqlServerOptions)}:UseInMemory"))
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var dbContext = serviceScope.ServiceProvider.GetRequiredService<IdentityContext>();

            logger.LogInformation("Updating database...");

            await dbContext.Database.MigrateAsync();

            logger.LogInformation("Updated database");
        }
    }
}
