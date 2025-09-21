using BuildingBlocks.Core.CQRS.Event.Internal;
using NextGen.Modules.Inventorys.Products.ValueObjects;

namespace NextGen.Modules.Inventorys.Products.Features.ChangingProductPrice;

public record ProductPriceChanged(Price Price) : DomainEvent;
