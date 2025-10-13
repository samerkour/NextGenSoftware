using BuildingBlocks.Abstractions.CQRS.Query;

namespace NextGen.Modules.Identity.Roles.Features.GetRoleClaims
{
    public record GetRoleClaimsQuery(Guid RoleId) : IQuery<IEnumerable<RoleClaimResponse>>;
}
