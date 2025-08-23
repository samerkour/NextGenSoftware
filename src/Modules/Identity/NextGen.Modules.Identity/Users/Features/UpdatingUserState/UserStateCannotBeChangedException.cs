using BuildingBlocks.Core.Exception.Types;
using BuildingBlocks.Core.Extensions;
using NextGen.Modules.Identity.Shared.Models;

namespace NextGen.Modules.Identity.Users.Features.UpdatingUserState;

internal class UserStateCannotBeChangedException : AppException
{
    public UserState State { get; }
    public Guid UserId { get; }

    public UserStateCannotBeChangedException(UserState state, Guid userId)
        : base($"User state cannot be changed to: '{state.ToName()}' for user with ID: '{userId}'.")
    {
        State = state;
        UserId = userId;
    }
}
