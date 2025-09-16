using BuildingBlocks.Abstractions.PagedList;
using BuildingBlocks.Core.CQRS.Query;
using BuildingBlocks.Core.PagedList;
using NextGen.Modules.Identity.Users.Dtos;

namespace NextGen.Modules.Identity.Users.Features.GettingUsers;

public record GetUsersResponse(ListResultModel<IdentityUserDto> IdentityUsers);
