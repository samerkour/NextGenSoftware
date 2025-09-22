using BuildingBlocks.Core.CQRS.Event.Internal;
using NextGen.Modules.Inventories.Products.ValueObjects;

namespace NextGen.Modules.Inventories.Products.Features.ReplenishingProductStock.Events.Domain;

public record ProductStockReplenished(ProductId ProductId, Stock NewStock, int ReplenishedQuantity) : DomainEvent;
