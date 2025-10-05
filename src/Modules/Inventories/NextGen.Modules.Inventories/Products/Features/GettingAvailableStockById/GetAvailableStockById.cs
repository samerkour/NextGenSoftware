using BuildingBlocks.Abstractions.CQRS.Query;

namespace NextGen.Modules.Inventories.Products.Features.GettingAvailableStockById;

public record GetAvailableStockById(long ProductId) : IQuery<int>;

