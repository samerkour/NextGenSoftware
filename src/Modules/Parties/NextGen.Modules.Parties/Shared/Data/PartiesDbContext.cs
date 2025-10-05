using BuildingBlocks.Core.Persistence.EfCore;
using NextGen.Modules.Parties.Parties.Models;
using NextGen.Modules.Parties.RestockSubscriptions.Models.Write;
using NextGen.Modules.Parties.Shared.Contracts;
using Microsoft.EntityFrameworkCore;

namespace NextGen.Modules.Parties.Shared.Data;

public class PartiesDbContext : EfDbContextBase, IPartiesDbContext
{
    public const string DefaultSchema = "party";

    public PartiesDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Party> Parties => Set<Party>();
    public DbSet<RestockSubscription> RestockSubscriptions => Set<RestockSubscription>();

    public override void Dispose()
    {
        base.Dispose();
    }
}
