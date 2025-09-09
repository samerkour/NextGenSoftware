using System.Net.NetworkInformation;

namespace NextGen.Modules.Identity.Identity.Features.Login;

public record LoginUserRequest(string Captcha, string UserNameOrEmail, string Password, bool Remember);
