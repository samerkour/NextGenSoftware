using Bogus;
using BuildingBlocks.Abstractions.Persistence;
using NextGen.Modules.Inventories.Products.Models;
using NextGen.Modules.Inventories.Products.ValueObjects;
using NextGen.Modules.Inventories.Shared.Contracts;
using Microsoft.EntityFrameworkCore;

namespace NextGen.Modules.Inventories.Products.Data;

public class ProductDataSeeder : IDataSeeder
{
    private readonly IInventoryDbContext _dbContext;

    public ProductDataSeeder(IInventoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SeedAllAsync()
    {
        if (await _dbContext.Products.AnyAsync())
            return;

        long id = 1;

        // https://github.com/bchavez/Bogus
        // https://www.youtube.com/watch?v=T9pwE1GAr_U
        var productFaker = new Faker<Product>().CustomInstantiator(faker =>
        {
            var brand = Product.Create(
                id,
                faker.Commerce.ProductName(),
                Stock.Create(faker.Random.Int(10, 20), 5, 20),
                ProductStatus.Available,
                Dimensions.Create(faker.Random.Int(10, 50), faker.Random.Int(10, 50), faker.Random.Int(10, 50)),
                faker.PickRandom<string>("M", "S", "L"),
                faker.Random.Enum<ProductColor>(),
                faker.Commerce.ProductDescription(),
                Price.Create(faker.PickRandom<decimal>(100, 200, 500)),
                faker.Random.Long(1, 3),
                faker.Random.Long(1, 5),
                faker.Random.Long(1, 5));
            id++;

            return brand;
        });
        var products = productFaker.Generate(5);

        await _dbContext.Products.AddRangeAsync(products);
        await _dbContext.SaveChangesAsync();
    }
}
