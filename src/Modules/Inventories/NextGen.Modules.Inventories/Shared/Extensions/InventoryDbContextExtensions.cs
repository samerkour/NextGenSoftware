using NextGen.Modules.Inventorys.Brands;
using NextGen.Modules.Inventorys.Categories;
using NextGen.Modules.Inventorys.Products.Models;
using NextGen.Modules.Inventorys.Products.ValueObjects;
using NextGen.Modules.Inventorys.Shared.Contracts;
using NextGen.Modules.Inventorys.Suppliers;
using Microsoft.EntityFrameworkCore;

namespace NextGen.Modules.Inventorys.Shared.Extensions;

/// <summary>
/// Put some shared code between multiple feature here, for preventing duplicate some codes
/// Ref: https://www.youtube.com/watch?v=01lygxvbao4.
/// </summary>
public static class InventoryDbContextExtensions
{
    public static Task<bool> ProductExistsAsync(
        this IInventoryDbContext context,
        ProductId id,
        CancellationToken cancellationToken = default)
    {
        return context.Products.AnyAsync(x => x.Id == id, cancellationToken: cancellationToken);
    }

    public static ValueTask<Product?> FindProductByIdAsync(
        this IInventoryDbContext context,
        ProductId id)
    {
        return context.Products.FindAsync(id);
    }

    public static Task<bool> SupplierExistsAsync(
        this IInventoryDbContext context,
        SupplierId id,
        CancellationToken cancellationToken = default)
    {
        return context.Suppliers.AnyAsync(x => x.Id == id, cancellationToken: cancellationToken);
    }

    public static ValueTask<Supplier?> FindSupplierByIdAsync(
        this IInventoryDbContext context,
        SupplierId id)
    {
        return context.Suppliers.FindAsync(id);
    }

    public static Task<bool> CategoryExistsAsync(
        this IInventoryDbContext context,
        CategoryId id,
        CancellationToken cancellationToken = default)
    {
        return context.Categories.AnyAsync(x => x.Id == id, cancellationToken: cancellationToken);
    }

    public static ValueTask<Category?> FindCategoryAsync(
        this IInventoryDbContext context,
        CategoryId id)
    {
        return context.Categories.FindAsync(id);
    }

    public static Task<bool> BrandExistsAsync(
        this IInventoryDbContext context,
        BrandId id,
        CancellationToken cancellationToken = default)
    {
        return context.Brands.AnyAsync(x => x.Id == id, cancellationToken: cancellationToken);
    }

    public static ValueTask<Brand?> FindBrandAsync(
        this IInventoryDbContext context,
        BrandId id)
    {
        return context.Brands.FindAsync(id);
    }
}
