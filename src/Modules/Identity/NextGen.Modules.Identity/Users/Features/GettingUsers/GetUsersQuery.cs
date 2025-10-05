using BuildingBlocks.Core.CQRS.Query;

namespace NextGen.Modules.Identity.Users.Features.GettingUsers;

public record GetUsersQuery : ListQuery<GetUsersResponse>;
