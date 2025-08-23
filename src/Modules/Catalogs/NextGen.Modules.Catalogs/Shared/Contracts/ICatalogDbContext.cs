using NextGen.Modules.Catalogs.Brands;
using NextGen.Modules.Catalogs.Categories;
using NextGen.Modules.Catalogs.Products.Models;
using NextGen.Modules.Catalogs.Suppliers;
using Microsoft.EntityFrameworkCore;

namespace NextGen.Modules.Catalogs.Shared.Contracts;

public interface ICatalogDbContext
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
