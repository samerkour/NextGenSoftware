using Ardalis.GuardClauses;
using NextGen.Modules.Catalogs.Categories.Exceptions.Application;

namespace NextGen.Modules.Catalogs.Categories;

public static class GuardExtensions
{
    public static void ExistsCategory(this IGuardClause guardClause, bool exists, long categoryId)
    {
        if (exists == false)
            throw new CategoryNotFoundException(categoryId);
    }
}
