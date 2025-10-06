using BuildingBlocks.Abstractions.CQRS.Command;

namespace NextGen.Modules.Identity.Claims.Features.UpdateClaim;

public record UpdateClaimCommand(
    Guid Id,
    string Type,
    string Value,
    Guid ClaimGroupId
) : ICommand<UpdateClaimResponse>;
