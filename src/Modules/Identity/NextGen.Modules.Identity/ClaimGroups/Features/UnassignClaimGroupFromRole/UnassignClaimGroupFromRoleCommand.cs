using BuildingBlocks.Abstractions.CQRS.Command;

namespace NextGen.Modules.Identity.ClaimGroups.Features.UnassignClaimGroupFromRole
{
    public record UnassignClaimGroupFromRoleCommand(Guid ClaimGroupId, Guid RoleId)
        : ICommand<UnassignClaimGroupFromRoleResponse>;
}
