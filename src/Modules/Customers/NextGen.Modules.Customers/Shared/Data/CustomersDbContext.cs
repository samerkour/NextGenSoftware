using BuildingBlocks.Core.Persistence.EfCore;
using NextGen.Modules.Customers.Customers.Models;
using NextGen.Modules.Customers.RestockSubscriptions.Models.Write;
using NextGen.Modules.Customers.Shared.Contracts;
using Microsoft.EntityFrameworkCore;

namespace NextGen.Modules.Customers.Shared.Data;

public class CustomersDbContext : EfDbContextBase, ICustomersDbContext
{
    public const string DefaultSchema = "customer";

    public CustomersDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<RestockSubscription> RestockSubscriptions => Set<RestockSubscription>();

    public override void Dispose()
    {
        base.Dispose();
    }
}
