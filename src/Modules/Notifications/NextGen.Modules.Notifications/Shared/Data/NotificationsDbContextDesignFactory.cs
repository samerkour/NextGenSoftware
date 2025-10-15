using BuildingBlocks.Persistence.EfCore.SqlServer;

namespace NextGen.Modules.Notifications.Shared.Data;

public class NotificationsDbContextDesignFactory : DbContextDesignFactoryBase<NotificationsDbContext>
{
    public NotificationsDbContextDesignFactory() : base("Notifications", "Notifications:SqlServerOptions")
    {
    }
}
