using BuildingBlocks.Core.CQRS.Event.Internal;

namespace NextGen.Modules.Catalogs.Products.Features.ChangingRestockThreshold;

public record RestockThresholdChanged(long ProductId, int RestockThreshold) : DomainEvent;
