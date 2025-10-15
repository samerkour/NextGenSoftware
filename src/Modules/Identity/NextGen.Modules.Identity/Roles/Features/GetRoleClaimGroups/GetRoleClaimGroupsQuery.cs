using BuildingBlocks.Abstractions.CQRS.Query;

namespace NextGen.Modules.Identity.Roles.Features.GetRoleClaimGroups
{
    public record GetRoleClaimGroupsQuery(Guid RoleId) : IQuery<IEnumerable<GetRoleClaimGroupsResponse>>;
}
