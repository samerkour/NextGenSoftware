using BuildingBlocks.Core.CQRS.Event.Internal;
using NextGen.Modules.Catalogs.Products.ValueObjects;

namespace NextGen.Modules.Catalogs.Products.Features.ChangingProductPrice;

public record ProductPriceChanged(Price Price) : DomainEvent;
