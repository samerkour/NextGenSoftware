namespace NextGen.Modules.Inventories.Products.Features.GettingProductsView;

public record struct ProductViewDto(long Id, string Name, string CategoryName, string SupplierName, long ItemCount);
