using NextGen.Modules.Identity.Shared.Models;

namespace NextGen.Modules.Identity.Identity.Features.Login;

public record LoginResponse

{
    public LoginResponse(ApplicationUser user, string tokenType, double expiresIn, string accessToken, string refreshToken)
    {
        UserId = user.Id;
        FirstName = user.FirstName;
        LastName = user.LastName;
        Username = user.UserName!;
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        TokenType = tokenType;
        ExpiresIn = expiresIn;
        ProfileImagePath = user.ProfileImagePath;
    }

    public Guid UserId { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string Username { get; }
    public string AccessToken { get; }
    public string RefreshToken { get; }
    public string TokenType { get; }
    public double ExpiresIn { get; }
    public string? ProfileImagePath { get;}
}
