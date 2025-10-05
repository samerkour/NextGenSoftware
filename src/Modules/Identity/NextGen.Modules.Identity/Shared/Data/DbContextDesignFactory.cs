using BuildingBlocks.Persistence.EfCore.SqlServer;

namespace NextGen.Modules.Identity.Shared.Data;

public class DbContextDesignFactory : DbContextDesignFactoryBase<IdentityContext>
{
    public DbContextDesignFactory() : base("Identity", "Identity:SqlServerOptions")
    {
    }
}
