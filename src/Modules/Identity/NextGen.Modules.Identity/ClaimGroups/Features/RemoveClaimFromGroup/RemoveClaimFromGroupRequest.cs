using BuildingBlocks.Abstractions.CQRS.Command;

namespace NextGen.Modules.Identity.ClaimGroups.Features.RemoveClaimFromGroup
{
    public record RemoveClaimFromGroupRequest(Guid GroupId, Guid ClaimId)
        : ICommand<RemoveClaimFromGroupResponse>;
}
