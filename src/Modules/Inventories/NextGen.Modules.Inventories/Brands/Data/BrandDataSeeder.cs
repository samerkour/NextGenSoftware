using Bogus;
using BuildingBlocks.Abstractions.Persistence;
using NextGen.Modules.Inventories.Shared.Contracts;
using Microsoft.EntityFrameworkCore;

namespace NextGen.Modules.Inventories.Brands.Data;

public class BrandDataSeeder : IDataSeeder
{
    private readonly IInventoryDbContext _context;

    public BrandDataSeeder(IInventoryDbContext context)
    {
        _context = context;
    }

    public async Task SeedAllAsync()
    {
        if (await _context.Brands.AnyAsync())
            return;

        long id = 1;

        // https://github.com/bchavez/Bogus
        // https://www.youtube.com/watch?v=T9pwE1GAr_U
        var brandFaker = new Faker<Brand>().CustomInstantiator(faker =>
        {
            var brand = Brand.Create(id, faker.Company.CompanyName());
            id++;
            return brand;
        });
        var brands = brandFaker.Generate(5);

        await _context.Brands.AddRangeAsync(brands);
        await _context.SaveChangesAsync();
    }
}
