using BuildingBlocks.Abstractions.CQRS.Command;

namespace NextGen.Modules.Inventories.Products.Features.ChangingMaxThreshold;

public record ChangeMaxThreshold(long ProductId, int NewMaxThreshold) : ITxCommand;
