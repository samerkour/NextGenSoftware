using Ardalis.GuardClauses;
using NextGen.Modules.Inventorys.Brands.Exceptions.Application;

namespace NextGen.Modules.Inventorys.Brands;

public static class GuardExtensions
{
    public static void ExistsBrand(this IGuardClause guardClause, bool exists, long brandId)
    {
        if (!exists)
            throw new BrandNotFoundException(brandId);
    }
}
