using BuildingBlocks.Core.CQRS.Query;
using NextGen.Modules.Identity.Roles.Dtos;

namespace NextGen.Modules.Identity.Roles.Features.GetRoles;

public record GetRolesResponse(ListResultModel<RoleDto> Roles);
