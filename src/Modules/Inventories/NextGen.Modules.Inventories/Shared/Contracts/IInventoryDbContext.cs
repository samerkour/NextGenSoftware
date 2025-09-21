using NextGen.Modules.Inventorys.Brands;
using NextGen.Modules.Inventorys.Categories;
using NextGen.Modules.Inventorys.Products.Models;
using NextGen.Modules.Inventorys.Suppliers;
using Microsoft.EntityFrameworkCore;

namespace NextGen.Modules.Inventorys.Shared.Contracts;

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
