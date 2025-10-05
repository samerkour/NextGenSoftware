using Ardalis.GuardClauses;
using NextGen.Modules.Inventories.Suppliers.Exceptions.Application;

namespace NextGen.Modules.Inventories.Suppliers;

public static class GuardExtensions
{
    public static void ExistsSupplier(this IGuardClause guardClause, bool exists, long supplierId)
    {
        if (exists == false)
            throw new SupplierNotFoundException(supplierId);
    }
}
