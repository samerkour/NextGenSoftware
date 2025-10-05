using BuildingBlocks.Core.Persistence.EfCore;
using NextGen.Modules.Inventories.Brands;
using NextGen.Modules.Inventories.Categories;
using NextGen.Modules.Inventories.Products.Models;
using NextGen.Modules.Inventories.Shared.Contracts;
using NextGen.Modules.Inventories.Suppliers;
using Microsoft.EntityFrameworkCore;

namespace NextGen.Modules.Inventories.Shared.Data;

public class InventoryDbContext : EfDbContextBase, IInventoryDbContext
{
    public const string DefaultSchema = "inventory";

    public InventoryDbContext(DbContextOptions options) : base(options)
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
