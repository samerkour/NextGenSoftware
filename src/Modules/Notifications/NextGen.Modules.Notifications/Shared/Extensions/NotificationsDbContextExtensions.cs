using NextGen.Modules.Notifications.Notifications.Models;
using NextGen.Modules.Notifications.Notifications.ValueObjects;
using NextGen.Modules.Notifications.Shared.Data;
using Microsoft.EntityFrameworkCore;

namespace NextGen.Modules.Notifications.Shared.Extensions;

public static class NotificationsDbContextExtensions
{
    public static ValueTask<Notif?> FindNotificationByIdAsync(this NotificationsDbContext context, NotifId id)
    {
        return context.Notifications.FindAsync(id);
    }

    public static Task<bool> ExistsNotificationByIdAsync(this NotificationsDbContext context, NotifId id)
    {
        return context.Notifications.AnyAsync(x => x.Id == id);
    }
}
