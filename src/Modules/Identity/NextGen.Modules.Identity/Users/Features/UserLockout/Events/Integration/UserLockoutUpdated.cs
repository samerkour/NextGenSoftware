using BuildingBlocks.Core.Messaging;
using NextGen.Modules.Identity.Shared.Models;

namespace NextGen.Modules.Identity.Users.Features.UpdatingUserLockout.Events.Integration;

public record UserLockoutUpdated(Guid UserId, DateTimeOffset? LockoutEnd);
