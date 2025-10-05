using BuildingBlocks.Core.Exception.Types;
using BuildingBlocks.Core.Extensions;
using NextGen.Modules.Identity.Shared.Models;

namespace NextGen.Modules.Identity.Users.Features.UpdatingUserLockout;

internal class UserCannotBeLockedOutException : AppException
{
    public string UserName { get; }

    public UserCannotBeLockedOutException(string userName)
        : base($"User with UserName : '{userName}' cannot be locked out. either the user is an admin or lockout is not enabled.")
    {
        UserName = userName;
    }
}
