using NextGen.Modules.Inventories.Brands;
using NextGen.Modules.Inventories.Categories;
using NextGen.Modules.Inventories.Products.Models;
using NextGen.Modules.Inventories.Products.ValueObjects;
using NextGen.Modules.Inventories.Shared.Contracts;
using NextGen.Modules.Inventories.Suppliers;
using Microsoft.EntityFrameworkCore;

namespace NextGen.Modules.Inventories.Shared.Extensions;

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
