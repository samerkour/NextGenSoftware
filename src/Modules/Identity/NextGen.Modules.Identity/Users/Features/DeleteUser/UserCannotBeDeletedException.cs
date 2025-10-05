using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingBlocks.Core.Exception.Types;

namespace NextGen.Modules.Identity.Users.Features.DeleteUser;

internal class UserCannotBeDeletedException : AppException
{
    public Guid UserId { get; }

    public UserCannotBeDeletedException(Guid userId)
        : base($"User with ID: '{userId}' cannot be deleted.")
    {
        UserId = userId;
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

public class DeleteUserException : BadRequestException
{
    public DeleteUserException(string error) : base(error) { }
}
