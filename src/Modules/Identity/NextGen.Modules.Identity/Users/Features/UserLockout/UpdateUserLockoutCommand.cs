using BuildingBlocks.Abstractions.CQRS.Command;
using NextGen.Modules.Identity.Shared.Models;

namespace NextGen.Modules.Identity.Users.Features.UpdatingUserLockout;

public record UpdateUserLockoutCommand(Guid UserId, DateTimeOffset? LockoutEnd) : ITxUpdateCommand<bool>;
