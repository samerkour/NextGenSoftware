using BuildingBlocks.Core.Exception.Types;

namespace NextGen.Modules.Identity.Identity.Features.VerifyEmail.Exceptions;

public class EmailAlreadyVerifiedException : ConflictException
{
    public EmailAlreadyVerifiedException(string email) : base($"User with email {email} already verified.")
    {
    }
}
