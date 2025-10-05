using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingBlocks.Core.Exception.Types;

namespace NextGen.Modules.Identity.Users.Features.ChangeUserPassword;

public class ChangeUserPasswordException : BadRequestException
{
    public ChangeUserPasswordException(string error)
        : base($"Failed to change password: {error}")
    {
    }
}

public class UserNotFoundException : NotFoundException
{
    public Guid UserId { get; }

    public UserNotFoundException(Guid userId)
        : base($"User with ID '{userId}' was not found.")
    {
        UserId = userId;
    }
}
