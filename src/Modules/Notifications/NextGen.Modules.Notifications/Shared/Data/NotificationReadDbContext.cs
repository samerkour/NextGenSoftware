using BuildingBlocks.Persistence.Mongo;
using NextGen.Modules.Notifications.Notifications.Models.Reads;
using Humanizer;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace NextGen.Modules.Notifications.Shared.Data;

public class NotificationReadDbContext : MongoDbContext
{
    public NotificationReadDbContext(IOptions<MongoOptions> options) : base(options)
    {
        Notifications = GetCollection<NotifReadModel>(nameof(Notifications).Underscore());
    }

    public IMongoCollection<NotifReadModel> Notifications { get; }
}
