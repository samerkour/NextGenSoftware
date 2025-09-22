using BuildingBlocks.Core.Messaging;

namespace NextGen.Modules.Inventories.Products.Features.ReplenishingProductStock.Events.Integration;

public record ProductStockReplenished(long ProductId, int NewStock, int ReplenishedQuantity) : IntegrationEvent;
