using NextGen.Modules.Sales.Sales.Models;
using NextGen.Modules.Sales.Sales.ValueObjects;
using NextGen.Modules.Sales.Shared.Data;
using Microsoft.EntityFrameworkCore;

namespace NextGen.Modules.Sales.Shared.Extensions;

public static class SalesDbContextExtensions
{
    public static ValueTask<Order?> FindSaleByIdAsync(this SalesDbContext context, OrderId id)
    {
        return context.Sales.FindAsync(id);
    }

    public static Task<bool> ExistsSaleByIdAsync(this SalesDbContext context, OrderId id)
    {
        return context.Sales.AnyAsync(x => x.Id == id);
    }
}
