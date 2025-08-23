using System.Reflection;
using BuildingBlocks.Core.Persistence.EfCore;
using NextGen.Modules.Orders.Orders.Models;
using NextGen.Modules.Orders.Shared.Contracts;
using Microsoft.EntityFrameworkCore;

namespace NextGen.Modules.Orders.Shared.Data;

public class OrdersDbContext : EfDbContextBase, IOrdersDbContext
{
    public const string DefaultSchema = "order";

    public OrdersDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Order> Orders => Set<Order>();
}
