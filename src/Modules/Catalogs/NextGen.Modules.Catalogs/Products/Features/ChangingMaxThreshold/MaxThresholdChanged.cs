using BuildingBlocks.Core.CQRS.Event.Internal;

namespace NextGen.Modules.Catalogs.Products.Features.ChangingMaxThreshold;

public record MaxThresholdChanged(long ProductId, int MaxThreshold) : DomainEvent;
