namespace NextGen.Modules.Identity.Identity.Features.RefreshingToken;

public record RefreshTokenRequest(string AccessToken, string RefreshToken);
