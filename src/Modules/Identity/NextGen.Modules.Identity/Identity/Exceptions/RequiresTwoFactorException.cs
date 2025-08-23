using BuildingBlocks.Core.Exception.Types;

namespace NextGen.Modules.Identity.Identity.Exceptions;

public class RequiresTwoFactorException : BadRequestException
{
    public RequiresTwoFactorException(string message) : base(message)
    {
    }
}
