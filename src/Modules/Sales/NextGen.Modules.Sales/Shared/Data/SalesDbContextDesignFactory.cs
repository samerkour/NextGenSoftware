using BuildingBlocks.Persistence.EfCore.SqlServer;

namespace NextGen.Modules.Sales.Shared.Data;

public class SalesDbContextDesignFactory : DbContextDesignFactoryBase<SalesDbContext>
{
    public SalesDbContextDesignFactory() : base("Sales", "Sales:SqlServerOptions")
    {
    }
}
