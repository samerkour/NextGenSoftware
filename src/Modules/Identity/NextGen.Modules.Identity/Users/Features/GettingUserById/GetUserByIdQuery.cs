using BuildingBlocks.Abstractions.CQRS.Query;

namespace NextGen.Modules.Identity.Users.Features.GettingUserById;

public record GetUserByIdQuery(Guid Id) : IQuery<UserByIdResponse>;
