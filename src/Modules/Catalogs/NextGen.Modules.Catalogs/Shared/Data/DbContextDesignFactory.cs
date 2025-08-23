using BuildingBlocks.Persistence.EfCore.SqlServer;

namespace NextGen.Modules.Catalogs.Shared.Data;

public class CatalogDbContextDesignFactory : DbContextDesignFactoryBase<CatalogDbContext>
{
    public CatalogDbContextDesignFactory() : base("Catalogs", "Catalogs:SqlServerOptions")
    {
    }
}
