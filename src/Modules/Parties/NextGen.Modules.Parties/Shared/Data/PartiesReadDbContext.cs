using BuildingBlocks.Persistence.Mongo;
using NextGen.Modules.Parties.Parties.Models.Reads;
using NextGen.Modules.Parties.RestockSubscriptions.Models.Read;
using Humanizer;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace NextGen.Modules.Parties.Shared.Data;

public class PartiesReadDbContext : MongoDbContext
{
    public PartiesReadDbContext(IOptions<MongoOptions> options) : base(options)
    {
        RestockSubscriptions = GetCollection<RestockSubscriptionReadModel>(nameof(RestockSubscriptions).Underscore());
        Parties = GetCollection<PartyReadModel>(nameof(Parties).Underscore());
    }

    public IMongoCollection<RestockSubscriptionReadModel> RestockSubscriptions { get; }
    public IMongoCollection<PartyReadModel> Parties { get; }
}
