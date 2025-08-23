using BuildingBlocks.Core.CQRS.Event.Internal;
using NextGen.Modules.Catalogs.Products.ValueObjects;

namespace NextGen.Modules.Catalogs.Products.Features.DebitingProductStock.Events.Domain;

public record ProductStockDebited(ProductId ProductId, Stock NewStock, int DebitedQuantity) : DomainEvent;
