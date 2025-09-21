using BuildingBlocks.Core.CQRS.Event.Internal;
using NextGen.Modules.Inventorys.Products.ValueObjects;

namespace NextGen.Modules.Inventorys.Products.Features.DebitingProductStock.Events.Domain;

public record ProductStockDebited(ProductId ProductId, Stock NewStock, int DebitedQuantity) : DomainEvent;
