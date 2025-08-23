using BuildingBlocks.Core.Persistence.EfCore;
using NextGen.Modules.Catalogs.Brands;
using NextGen.Modules.Catalogs.Categories;
using NextGen.Modules.Catalogs.Products.Models;
using NextGen.Modules.Catalogs.Shared.Contracts;
using NextGen.Modules.Catalogs.Suppliers;
using Microsoft.EntityFrameworkCore;

namespace NextGen.Modules.Catalogs.Shared.Data;

public class CatalogDbContext : EfDbContextBase, ICatalogDbContext
{
    public const string DefaultSchema = "catalog";

    public CatalogDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductView> ProductsView => Set<ProductView>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Brand> Brands => Set<Brand>();
}
