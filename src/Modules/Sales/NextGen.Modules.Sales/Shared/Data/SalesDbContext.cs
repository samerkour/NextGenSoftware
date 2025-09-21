using System.Reflection;
using BuildingBlocks.Core.Persistence.EfCore;
using NextGen.Modules.Sales.Sales.Models;
using NextGen.Modules.Sales.Shared.Contracts;
using Microsoft.EntityFrameworkCore;

namespace NextGen.Modules.Sales.Shared.Data;

public class SalesDbContext : EfDbContextBase, ISalesDbContext
{
    public const string DefaultSchema = "sale";

    public SalesDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Order> Sales => Set<Order>();
}
