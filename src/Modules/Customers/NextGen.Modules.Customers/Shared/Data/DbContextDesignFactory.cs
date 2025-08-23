using BuildingBlocks.Persistence.EfCore.SqlServer;

namespace NextGen.Modules.Customers.Shared.Data;

public class CustomerDbContextDesignFactory : DbContextDesignFactoryBase<CustomersDbContext>
{
    public CustomerDbContextDesignFactory() : base("Customers", "Customers:SqlServerOptions")
    {
    }
}
