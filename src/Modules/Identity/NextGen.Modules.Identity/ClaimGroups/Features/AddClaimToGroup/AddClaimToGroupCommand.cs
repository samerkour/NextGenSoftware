using BuildingBlocks.Abstractions.CQRS.Command;

namespace NextGen.Modules.Identity.ClaimGroups.Features.AddClaimToGroup
{
    public record AddClaimToGroupCommand(Guid GroupId, Guid ClaimId)
        : ICommand<AddClaimToGroupResponse>;
}
