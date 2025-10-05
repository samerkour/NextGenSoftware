using BuildingBlocks.Core.Exception.Types;
using NextGen.Modules.Identity.Shared.Models;

namespace NextGen.Modules.Identity.Identity.Exceptions;

public class RefreshTokenNotFoundException : NotFoundException
{
    public RefreshTokenNotFoundException(RefreshToken? refreshToken) : base("Refresh token not found.")
    {
    }
}
