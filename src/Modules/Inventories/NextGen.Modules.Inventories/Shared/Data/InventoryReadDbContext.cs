using BuildingBlocks.Persistence.Mongo;
using NextGen.Modules.Inventories.Products.Models.Read;
using Humanizer;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace NextGen.Modules.Inventories.Shared.Data;

public class InventoryReadDbContext : MongoDbContext
{
    public InventoryReadDbContext(IOptions<MongoOptions> options) : base(options)
    {
        Products = GetCollection<ProductReadModel>(nameof(Products).Underscore());
    }

    public IMongoCollection<ProductReadModel> Products { get; }
}
