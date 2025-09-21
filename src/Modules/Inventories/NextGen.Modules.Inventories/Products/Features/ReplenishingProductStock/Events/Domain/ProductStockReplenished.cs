using BuildingBlocks.Core.CQRS.Event.Internal;
using NextGen.Modules.Inventorys.Products.ValueObjects;

namespace NextGen.Modules.Inventorys.Products.Features.ReplenishingProductStock.Events.Domain;

public record ProductStockReplenished(ProductId ProductId, Stock NewStock, int ReplenishedQuantity) : DomainEvent;
