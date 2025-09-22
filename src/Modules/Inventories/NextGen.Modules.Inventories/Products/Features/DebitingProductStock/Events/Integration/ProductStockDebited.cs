using BuildingBlocks.Core.Messaging;

namespace NextGen.Modules.Inventories.Products.Features.DebitingProductStock.Events.Integration;

public record ProductStockDebited(long ProductId, int NewStock, int DebitedQuantity) : IntegrationEvent;
