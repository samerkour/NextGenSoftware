using Ardalis.GuardClauses;
using NextGen.Modules.Inventorys.Categories.Exceptions.Application;

namespace NextGen.Modules.Inventorys.Categories;

public static class GuardExtensions
{
    public static void ExistsCategory(this IGuardClause guardClause, bool exists, long categoryId)
    {
        if (exists == false)
            throw new CategoryNotFoundException(categoryId);
    }
}
