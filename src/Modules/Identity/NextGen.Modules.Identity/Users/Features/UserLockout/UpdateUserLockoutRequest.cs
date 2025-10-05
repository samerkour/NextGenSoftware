using NextGen.Modules.Identity.Shared.Models;

namespace NextGen.Modules.Identity.Users.Features.UpdatingUserLockout;

public record UpdateUserLockoutRequest
{
    public DateTimeOffset? LockoutEnd { get; init; }
}
