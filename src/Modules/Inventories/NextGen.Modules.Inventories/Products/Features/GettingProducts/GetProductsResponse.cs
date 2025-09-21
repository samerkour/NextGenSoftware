using BuildingBlocks.Core.CQRS.Query;
using NextGen.Modules.Inventorys.Products.Dtos;

namespace NextGen.Modules.Inventorys.Products.Features.GettingProducts;

public record GetProductsResponse(ListResultModel<ProductDto> Products);
