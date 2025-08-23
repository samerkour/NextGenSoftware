using BuildingBlocks.Core.CQRS.Event.Internal;
using NextGen.Modules.Catalogs.Products.ValueObjects;

namespace NextGen.Modules.Catalogs.Products.Features.ReplenishingProductStock.Events.Domain;

public record ProductStockReplenished(ProductId ProductId, Stock NewStock, int ReplenishedQuantity) : DomainEvent;
