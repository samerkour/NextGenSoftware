using NextGen.Modules.Identity.Roles.Dtos;
using BuildingBlocks.Core.CQRS.Query;

namespace NextGen.Modules.Identity.Roles.Features.GetRoles;

public record GetRolesResponse(ListResultModel<RoleDto> Roles);
