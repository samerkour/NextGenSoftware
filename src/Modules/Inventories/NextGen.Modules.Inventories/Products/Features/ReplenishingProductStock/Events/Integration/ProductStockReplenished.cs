using BuildingBlocks.Core.Messaging;

namespace NextGen.Modules.Inventorys.Products.Features.ReplenishingProductStock.Events.Integration;

public record ProductStockReplenished(long ProductId, int NewStock, int ReplenishedQuantity) : IntegrationEvent;
