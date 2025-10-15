using BuildingBlocks.Abstractions.Persistence.EfCore;
using NextGen.Modules.Notifications.Notifications.Models;
using Microsoft.EntityFrameworkCore;

namespace NextGen.Modules.Notifications.Shared.Contracts;

public interface INotificationsDbContext : IDbContext
{
    public DbSet<Notif> Notifications { get; }
}
