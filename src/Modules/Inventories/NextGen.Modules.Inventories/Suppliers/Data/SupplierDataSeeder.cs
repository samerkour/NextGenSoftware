using Bogus;
using BuildingBlocks.Abstractions.Persistence;
using NextGen.Modules.Inventories.Shared.Contracts;
using Microsoft.EntityFrameworkCore;

namespace NextGen.Modules.Inventories.Suppliers.Data;

public class SupplierDataSeeder : IDataSeeder
{
    private readonly IInventoryDbContext _dbContext;

    public SupplierDataSeeder(IInventoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SeedAllAsync()
    {
        if (await _dbContext.Suppliers.AnyAsync())
            return;

        long id = 1;
        var suppliersFaker = new Faker<Supplier>().CustomInstantiator(faker =>
        {
            var supplier = new Supplier(id, faker.Person.FullName);
            id++;
            return supplier;
        });

        var suppliers = suppliersFaker.Generate(5);
        await _dbContext.Suppliers.AddRangeAsync(suppliers);

        await _dbContext.SaveChangesAsync();
    }
}
