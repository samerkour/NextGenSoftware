using BuildingBlocks.Abstractions.CQRS.Command;
using NextGen.Modules.Identity.Roles.Features.GetRoleClaims;

namespace NextGen.Modules.Identity.Roles.Features.AddClaimToRole
{
    public record AddClaimToRoleCommand(Guid RoleId, Guid ClaimId)
        : ICommand<IEnumerable<RoleClaimResponse>>;
}
