using BuildingBlocks.Core.CQRS.Query;
using NextGen.Modules.Inventories.Products.Dtos;

namespace NextGen.Modules.Inventories.Products.Features.GettingProducts;

public record GetProductsResponse(ListResultModel<ProductDto> Products);
