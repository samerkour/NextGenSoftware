using NextGen.Modules.Orders.Orders.Models;
using NextGen.Modules.Orders.Orders.ValueObjects;
using NextGen.Modules.Orders.Shared.Data;
using Microsoft.EntityFrameworkCore;

namespace NextGen.Modules.Orders.Shared.Extensions;

public static class OrdersDbContextExtensions
{
    public static ValueTask<Order?> FindOrderByIdAsync(this OrdersDbContext context, OrderId id)
    {
        return context.Orders.FindAsync(id);
    }

    public static Task<bool> ExistsOrderByIdAsync(this OrdersDbContext context, OrderId id)
    {
        return context.Orders.AnyAsync(x => x.Id == id);
    }
}
