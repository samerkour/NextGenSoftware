using BuildingBlocks.Persistence.Mongo;
using NextGen.Modules.Sales.Sales.Models.Reads;
using Humanizer;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace NextGen.Modules.Sales.Shared.Data;

public class SaleReadDbContext : MongoDbContext
{
    public SaleReadDbContext(IOptions<MongoOptions> options) : base(options)
    {
        Sales = GetCollection<OrderReadModel>(nameof(Sales).Underscore());
    }

    public IMongoCollection<OrderReadModel> Sales { get; }
}
