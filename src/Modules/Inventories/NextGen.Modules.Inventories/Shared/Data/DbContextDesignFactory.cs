using BuildingBlocks.Persistence.EfCore.SqlServer;

namespace NextGen.Modules.Inventorys.Shared.Data;

public class InventoryDbContextDesignFactory : DbContextDesignFactoryBase<InventoryDbContext>
{
    public InventoryDbContextDesignFactory() : base("Inventorys", "Inventorys:SqlServerOptions")
    {
    }
}
