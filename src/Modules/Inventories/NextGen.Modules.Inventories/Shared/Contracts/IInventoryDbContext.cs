using NextGen.Modules.Inventories.Brands;
using NextGen.Modules.Inventories.Categories;
using NextGen.Modules.Inventories.Products.Models;
using NextGen.Modules.Inventories.Suppliers;
using Microsoft.EntityFrameworkCore;

namespace NextGen.Modules.Inventories.Shared.Contracts;

public interface IInventoryDbContext
{
    DbSet<Product> Products { get; }
    DbSet<Category> Categories { get; }
    DbSet<Brand> Brands { get; }
    DbSet<Supplier> Suppliers { get; }
    DbSet<ProductView> ProductsView { get; }

    DbSet<TEntity> Set<TEntity>()
        where TEntity : class;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
