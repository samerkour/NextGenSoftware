using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingBlocks.Core.Exception.Types;

namespace NextGen.Modules.Identity.Users.Features.UpdatingUser;

public class UpdateIdentityUserException : BadRequestException
{
    public Guid UserId { get; }
    public string Error { get; }

    public UpdateIdentityUserException(Guid userId, string error)
        : base($"Failed to update user with Id '{userId}'. Error: {error}")
    {
        UserId = userId;
        Error = error;
    }
}

public class UserNotFoundException : NotFoundException
{
    public Guid UserId { get; }

    public UserNotFoundException(Guid userId)
        : base($"User with Id '{userId}' was not found.")
    {
        UserId = userId;
    }
}
