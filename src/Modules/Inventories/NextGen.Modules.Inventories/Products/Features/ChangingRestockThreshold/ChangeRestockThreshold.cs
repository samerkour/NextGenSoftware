using BuildingBlocks.Abstractions.CQRS.Command;

namespace NextGen.Modules.Inventorys.Products.Features.ChangingRestockThreshold;

public record ChangeRestockThreshold(long ProductId, int NewRestockThreshold) : ITxCommand;
