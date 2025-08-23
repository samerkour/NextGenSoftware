using BuildingBlocks.Persistence.Mongo;
using NextGen.Modules.Orders.Orders.Models.Reads;
using Humanizer;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace NextGen.Modules.Orders.Shared.Data;

public class OrderReadDbContext : MongoDbContext
{
    public OrderReadDbContext(IOptions<MongoOptions> options) : base(options)
    {
        Orders = GetCollection<OrderReadModel>(nameof(Orders).Underscore());
    }

    public IMongoCollection<OrderReadModel> Orders { get; }
}
