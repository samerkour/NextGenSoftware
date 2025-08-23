using BuildingBlocks.Persistence.EfCore.SqlServer;

namespace NextGen.Modules.Orders.Shared.Data;

public class OrdersDbContextDesignFactory : DbContextDesignFactoryBase<OrdersDbContext>
{
    public OrdersDbContextDesignFactory() : base("Orders", "Orders:SqlServerOptions")
    {
    }
}
