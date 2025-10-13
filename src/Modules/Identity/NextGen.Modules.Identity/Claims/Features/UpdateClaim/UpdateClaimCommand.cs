using BuildingBlocks.Abstractions.CQRS.Command;

namespace NextGen.Modules.Identity.Claims.Features.UpdateClaim;

public record UpdateClaimCommand(
    Guid Id,
    string Type,
    string Value,
    string Name,
    string? Description,
    Guid ClaimGroupId
) : ICommand<UpdateClaimResponse>;
