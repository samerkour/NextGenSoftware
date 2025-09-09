using BuildingBlocks.Core.Exception.Types;

namespace NextGen.Modules.Identity.Identity.Features.Login;

public class LoginFailedException : AppException
{
    public LoginFailedException(string userNameOrEmail, string message) : base($"Login failed for username: {userNameOrEmail} => {message}")
    {
        UserNameOrEmail = userNameOrEmail;
        Message = message;
    }

    public string UserNameOrEmail { get; }
    public string Message { get; }
}
