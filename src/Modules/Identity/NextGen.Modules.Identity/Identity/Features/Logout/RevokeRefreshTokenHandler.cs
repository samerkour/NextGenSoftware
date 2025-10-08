using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Command;
using NextGen.Modules.Identity.Identity.Exceptions;
using NextGen.Modules.Identity.Identity.Features.RefreshingToken;
using NextGen.Modules.Identity.Shared.Data;
using Microsoft.EntityFrameworkCore;

namespace NextGen.Modules.Identity.Identity.Features.Logout;

internal class RevokeRefreshTokenHandler : ICommandHandler<RevokeRefreshTokenCommand>
{
    private readonly IdentityContext _context;

    public RevokeRefreshTokenHandler(IdentityContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        RevokeRefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(RevokeRefreshTokenCommand));

        var refreshToken = await _context.Set<Shared.Models.RefreshToken>()
            .FirstOrDefaultAsync(x => x.Token == request.RefreshToken, cancellationToken: cancellationToken);

        if (refreshToken == null)
            throw new RefreshTokenNotFoundException(refreshToken);

        if (!refreshToken.IsRefreshTokenValid())
            throw new InvalidRefreshTokenException(refreshToken);

        // revoke token and save
        refreshToken.RevokedAt = DateTime.Now;
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
