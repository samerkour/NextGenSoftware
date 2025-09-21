using Ardalis.GuardClauses;
using NextGen.Modules.Inventorys.Suppliers.Exceptions.Application;

namespace NextGen.Modules.Inventorys.Suppliers;

public static class GuardExtensions
{
    public static void ExistsSupplier(this IGuardClause guardClause, bool exists, long supplierId)
    {
        if (exists == false)
            throw new SupplierNotFoundException(supplierId);
    }
}
