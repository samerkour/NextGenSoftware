using NextGen.Modules.Parties.Parties.Models;
using Microsoft.EntityFrameworkCore;

namespace NextGen.Modules.Parties.Shared.Contracts;

public interface IPartiesDbContext
{
    DbSet<TEntity> Set<TEntity>()
        where TEntity : class;

    public DbSet<Party> Parties { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
