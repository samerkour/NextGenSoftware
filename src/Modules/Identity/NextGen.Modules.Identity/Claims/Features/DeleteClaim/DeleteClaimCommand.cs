using BuildingBlocks.Abstractions.CQRS.Command;

namespace NextGen.Modules.Identity.Claims.Features.DeleteClaim;

public record DeleteClaimCommand(Guid Id) : ICommand<DeleteClaimResponse>;
