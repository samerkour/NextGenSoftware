using BuildingBlocks.Core.CQRS.Event.Internal;
using NextGen.Modules.Inventories.Products.ValueObjects;

namespace NextGen.Modules.Inventories.Products.Features.DebitingProductStock.Events.Domain;

public record ProductStockDebited(ProductId ProductId, Stock NewStock, int DebitedQuantity) : DomainEvent;
