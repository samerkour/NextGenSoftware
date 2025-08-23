using BuildingBlocks.Core.CQRS.Query;
using NextGen.Modules.Catalogs.Products.Dtos;

namespace NextGen.Modules.Catalogs.Products.Features.GettingProducts;

public record GetProductsResponse(ListResultModel<ProductDto> Products);
