using BuildingBlocks.Abstractions.CQRS.Command;

namespace NextGen.Modules.Identity.Claims.Features.CreateClaim
{
    public record CreateClaimCommand(
        string Type,
        string Value,
        Guid ClaimGroupId
    ) : ICommand<CreateClaimResponse>;
}
