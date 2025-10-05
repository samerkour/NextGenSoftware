using BuildingBlocks.Core.CQRS.Event.Internal;
using NextGen.Modules.Inventories.Products.ValueObjects;

namespace NextGen.Modules.Inventories.Products.Features.ChangingProductPrice;

public record ProductPriceChanged(Price Price) : DomainEvent;
