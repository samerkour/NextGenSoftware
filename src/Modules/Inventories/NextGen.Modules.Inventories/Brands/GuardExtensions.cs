using Ardalis.GuardClauses;
using NextGen.Modules.Inventories.Brands.Exceptions.Application;

namespace NextGen.Modules.Inventories.Brands;

public static class GuardExtensions
{
    public static void ExistsBrand(this IGuardClause guardClause, bool exists, long brandId)
    {
        if (!exists)
            throw new BrandNotFoundException(brandId);
    }
}
