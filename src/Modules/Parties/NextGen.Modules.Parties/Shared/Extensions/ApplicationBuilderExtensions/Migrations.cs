using BuildingBlocks.Persistence.EfCore.SqlServer;
using NextGen.Modules.Parties.Shared.Data;
using Microsoft.EntityFrameworkCore;

namespace NextGen.Modules.Parties.Shared.Extensions.ApplicationBuilderExtensions;

public static partial class ApplicationBuilderExtensions
{
    public static async Task ApplyDatabaseMigrations(this IApplicationBuilder app, ILogger logger)
    {
        var configuration = app.ApplicationServices.GetRequiredService<IConfiguration>();
        if (!configuration.GetValue<bool>(
                $"{PartiesModuleConfiguration.ModuleName}:{nameof(SqlServerOptions)}:UseInMemory"))
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var dbContext = serviceScope.ServiceProvider.GetRequiredService<PartiesDbContext>();

            logger.LogInformation("Updating inventory database...");

            await dbContext.Database.MigrateAsync();

            logger.LogInformation("Updated inventory database");
        }
    }
}
