using BuildingBlocks.Core.Persistence.EfCore;
using NextGen.Modules.Inventorys.Brands;
using NextGen.Modules.Inventorys.Categories;
using NextGen.Modules.Inventorys.Products.Models;
using NextGen.Modules.Inventorys.Shared.Contracts;
using NextGen.Modules.Inventorys.Suppliers;
using Microsoft.EntityFrameworkCore;

namespace NextGen.Modules.Inventorys.Shared.Data;

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
