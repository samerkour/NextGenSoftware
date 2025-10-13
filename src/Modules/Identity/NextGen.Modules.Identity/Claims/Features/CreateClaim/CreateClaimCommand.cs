using BuildingBlocks.Abstractions.CQRS.Command;

namespace NextGen.Modules.Identity.Claims.Features.CreateClaim
{
    public record CreateClaimCommand(
        string Type,
        string Value,
        string Name,
        string? Description,
        Guid ClaimGroupId
    ) : ICommand<CreateClaimResponse>;
}
