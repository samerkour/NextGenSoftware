using BuildingBlocks.Persistence.EfCore.SqlServer;

namespace NextGen.Modules.Parties.Shared.Data;

public class PartyDbContextDesignFactory : DbContextDesignFactoryBase<PartiesDbContext>
{
    public PartyDbContextDesignFactory() : base("Parties", "Parties:SqlServerOptions")
    {
    }
}
