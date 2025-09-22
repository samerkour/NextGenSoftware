using Ardalis.GuardClauses;
using NextGen.Modules.Inventories.Products.Exceptions.Application;

namespace NextGen.Modules.Inventories.Products;

public static class GuardExtensions
{
    public static void ExistsProduct(this IGuardClause guardClause, bool exists, long productId)
    {
        if (exists == false)
            throw new ProductNotFoundException(productId);
    }
}
