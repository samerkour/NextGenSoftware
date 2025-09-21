using BuildingBlocks.Abstractions.CQRS.Query;

namespace NextGen.Modules.Inventorys.Products.Features.GettingAvailableStockById;

public record GetAvailableStockById(long ProductId) : IQuery<int>;

