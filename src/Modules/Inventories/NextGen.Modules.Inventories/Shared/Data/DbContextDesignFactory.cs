using BuildingBlocks.Persistence.EfCore.SqlServer;

namespace NextGen.Modules.Inventories.Shared.Data;

public class InventoryDbContextDesignFactory : DbContextDesignFactoryBase<InventoryDbContext>
{
    public InventoryDbContextDesignFactory() : base("Inventories", "Inventories:SqlServerOptions")
    {
    }
}
