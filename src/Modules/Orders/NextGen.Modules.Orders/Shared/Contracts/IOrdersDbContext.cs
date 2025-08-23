using BuildingBlocks.Abstractions.Persistence.EfCore;
using NextGen.Modules.Orders.Orders.Models;
using Microsoft.EntityFrameworkCore;

namespace NextGen.Modules.Orders.Shared.Contracts;

public interface IOrdersDbContext : IDbContext
{
    public DbSet<Order> Orders { get; }
}
