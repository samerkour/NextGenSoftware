using BuildingBlocks.Abstractions.Persistence;
using NextGen.Modules.Inventories.Shared.Contracts;
using Microsoft.EntityFrameworkCore;

namespace NextGen.Modules.Inventories.Categories.Data;

public class CategoryDataSeeder : IDataSeeder
{
    private readonly IInventoryDbContext _dbContext;

    public CategoryDataSeeder(IInventoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SeedAllAsync()
    {
        if (await _dbContext.Categories.AnyAsync())
            return;

        await _dbContext.Categories.AddRangeAsync(new List<Category>
        {
            Category.Create(1, "Electronics", "0001", "All electronic goods"),
            Category.Create(2, "Clothing", "0002", "All clothing goods"),
            Category.Create(3, "Books", "0003", "All books"),
        });
        await _dbContext.SaveChangesAsync();
    }
}
