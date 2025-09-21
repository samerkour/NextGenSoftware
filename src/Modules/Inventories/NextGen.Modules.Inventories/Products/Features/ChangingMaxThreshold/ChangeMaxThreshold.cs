using BuildingBlocks.Abstractions.CQRS.Command;

namespace NextGen.Modules.Inventorys.Products.Features.ChangingMaxThreshold;

public record ChangeMaxThreshold(long ProductId, int NewMaxThreshold) : ITxCommand;
