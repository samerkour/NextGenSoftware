using BuildingBlocks.Abstractions.Persistence.EfCore;
using NextGen.Modules.Sales.Sales.Models;
using Microsoft.EntityFrameworkCore;

namespace NextGen.Modules.Sales.Shared.Contracts;

public interface ISalesDbContext : IDbContext
{
    public DbSet<Order> Sales { get; }
}
