using BuildingBlocks.Core.Exception.Types;

namespace NextGen.Modules.Identity.Identity.Exceptions;

public class PasswordIsInvalidException : AppException
{
    public PasswordIsInvalidException(string message) : base(message)
    {
    }
}
