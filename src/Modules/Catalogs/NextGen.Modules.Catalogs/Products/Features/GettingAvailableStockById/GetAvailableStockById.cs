using BuildingBlocks.Abstractions.CQRS.Query;

namespace NextGen.Modules.Catalogs.Products.Features.GettingAvailableStockById;

public record GetAvailableStockById(long ProductId) : IQuery<int>;

