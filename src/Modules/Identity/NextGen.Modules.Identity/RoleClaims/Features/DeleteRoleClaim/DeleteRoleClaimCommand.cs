using BuildingBlocks.Abstractions.CQRS.Command;

namespace NextGen.Modules.Identity.RoleClaims.Features.DeleteRoleClaim
{
    public record DeleteRoleClaimCommand(Guid RoleId, Guid ClaimId)
        : ICommand<DeleteRoleClaimResponse>;
}
