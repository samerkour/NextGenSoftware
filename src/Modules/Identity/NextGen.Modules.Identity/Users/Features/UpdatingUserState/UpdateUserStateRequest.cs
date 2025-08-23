using NextGen.Modules.Identity.Shared.Models;

namespace NextGen.Modules.Identity.Users.Features.UpdatingUserState;

public record UpdateUserStateRequest
{
    public UserState UserState { get; init; }
}
