using BuildingBlocks.Core.Exception.Types;
using NextGen.Modules.Identity.Shared.Models;

namespace NextGen.Modules.Identity.Identity.Features.RefreshingToken;

public class InvalidRefreshTokenException : BadRequestException
{
    public InvalidRefreshTokenException(Shared.Models.RefreshToken? refreshToken) : base($"refresh token {refreshToken?.Token} is invalid!")
    {
    }
}
