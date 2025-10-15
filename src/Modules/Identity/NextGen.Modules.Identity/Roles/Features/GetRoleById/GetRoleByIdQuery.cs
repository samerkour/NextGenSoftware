using BuildingBlocks.Abstractions.CQRS.Query;

namespace NextGen.Modules.Identity.Roles.Features.GetRoleById
{
    public record GetRoleByIdQuery(Guid RoleId) : IQuery<GetRoleByIdResponse>;
}
