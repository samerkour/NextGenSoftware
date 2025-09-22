namespace NextGen.Modules.Inventories;

public static class InventoryCacheKey
{
    public static string ProductsByCategory(long categoryId) => $"{nameof(ProductsByCategory)}{categoryId}";
    public static string ProductsWithDiscounts(long id) => $"{nameof(ProductsWithDiscounts)}{id}";
}
