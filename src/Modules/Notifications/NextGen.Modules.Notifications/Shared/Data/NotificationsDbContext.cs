using BuildingBlocks.Core.Persistence.EfCore;
using Microsoft.EntityFrameworkCore;
using NextGen.Modules.Notifications.Notifications.Models;
using NextGen.Modules.Notifications.Shared.Contracts;

namespace NextGen.Modules.Notifications.Shared.Data;

public class NotificationsDbContext : EfDbContextBase, INotificationsDbContext
{
    public const string DefaultSchema = "notification";

    public NotificationsDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Notif> Notifications => Set<Notif>();
}
