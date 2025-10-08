using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Core.Utils;
using BuildingBlocks.Security.Jwt;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NextGen.Modules.Identity.Identity.Dtos;
using NextGen.Modules.Identity.Identity.Features.RefreshingToken;
using NextGen.Modules.Identity.Shared.Data;

namespace NextGen.Modules.Identity.Identity.Features.GenerateRefreshToken;

public record GenerateRefreshToken : ICommand<GenerateRefreshTokenResponse>
{
    public Guid UserId { get; init; }
    public string? Token { get; init; } // optional (rotation scenario)
}

public class GenerateRefreshTokenHandler : ICommandHandler<GenerateRefreshToken, GenerateRefreshTokenResponse>
{
    private readonly IdentityContext _context;
    private readonly RefreshTokenOptions _options;

    public GenerateRefreshTokenHandler(IdentityContext context, IOptions<RefreshTokenOptions> options)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _options = options?.Value ?? new RefreshTokenOptions();
    }

    public async Task<GenerateRefreshTokenResponse> Handle(
        GenerateRefreshToken request,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        Guard.Against.Default(request.UserId, nameof(request.UserId));

        var utcNow = DateTime.UtcNow;
        var ip = IpUtilities.GetIpAddress() ?? "unknown";

        // Try to find existing token (when client passed Token for rotation)
        Shared.Models.RefreshToken? refreshToken = null;
        if (!string.IsNullOrWhiteSpace(request.Token))
        {
            refreshToken = await _context.Set<Shared.Models.RefreshToken>()
                .FirstOrDefaultAsync(rt => rt.UserId == request.UserId && rt.Token == request.Token, cancellationToken);
        }

        if (refreshToken == null)
        {
            // Create new refresh token
            var tokenValue = Shared.Models.RefreshToken.GetRefreshToken();

            refreshToken = new Shared.Models.RefreshToken
            {
                UserId = request.UserId,
                Token = tokenValue,
                CreatedAt = utcNow,
                ExpiredAt = utcNow.AddDays(_options.LifetimeDays),
                CreatedByIp = ip
            };

            await _context.Set<Shared.Models.RefreshToken>().AddAsync(refreshToken, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            // Existing token found — validate then rotate
            if (!refreshToken.IsRefreshTokenValid())
                throw new InvalidRefreshTokenException(refreshToken);

            var tokenValue = Shared.Models.RefreshToken.GetRefreshToken();

            // rotate
            refreshToken.Token = tokenValue;
            refreshToken.CreatedAt = utcNow;
            refreshToken.ExpiredAt = utcNow.AddDays(_options.LifetimeDays);
            refreshToken.CreatedByIp = ip;
            refreshToken.RevokedAt = null; // ensure active
            _context.Set<Shared.Models.RefreshToken>().Update(refreshToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        // Cleanup old/invalid tokens for this user according to policy (soft revoke or hard delete)
        await CleanupOldRefreshTokens(request.UserId, utcNow, cancellationToken);

        var dto = new RefreshTokenDto
        {
            UserId = refreshToken.UserId,
            Token = refreshToken.Token,
            CreatedAt = refreshToken.CreatedAt,
            ExpireAt = refreshToken.ExpiredAt,
            CreatedByIp = refreshToken.CreatedByIp,
            IsExpired = refreshToken.IsExpired,
            IsRevoked = refreshToken.IsRevoked,
            IsActive = refreshToken.IsActive,
            RevokedAt = refreshToken.RevokedAt
        };

        return new GenerateRefreshTokenResponse(dto);
    }

    private async Task CleanupOldRefreshTokens(Guid userId, DateTime utcNow, CancellationToken cancellationToken)
    {
        // load tokens for the user
        var tokens = await _context.Set<Shared.Models.RefreshToken>()
            .Where(rt => rt.UserId == userId)
            .OrderByDescending(rt => rt.CreatedAt)
            .ToListAsync(cancellationToken);

        if (!tokens.Any())
            return;

        // 1) Soft-revoke or remove expired/invalid tokens
        var invalidTokens = tokens.Where(rt => !rt.IsRefreshTokenValid()).ToList();

        if (_options.SoftRevoke)
        {
            foreach (var t in invalidTokens)
            {
                if (t.RevokedAt == null)
                {
                    t.RevokedAt = utcNow;
                    _context.Set<Shared.Models.RefreshToken>().Update(t);
                }
            }
        }
        else
        {
            if (invalidTokens.Any())
                _context.Set<Shared.Models.RefreshToken>().RemoveRange(invalidTokens);
        }

        // 2) Optionally enforce MaxActiveTokensPerUser (revoke older ones)
        if (_options.MaxActiveTokensPerUser > 0)
        {
            var activeTokens = tokens.Where(rt => rt.IsRefreshTokenValid()).OrderByDescending(rt => rt.CreatedAt).ToList();
            if (activeTokens.Count > _options.MaxActiveTokensPerUser)
            {
                var tokensToRevoke = activeTokens.Skip(_options.MaxActiveTokensPerUser).ToList();
                if (_options.SoftRevoke)
                {
                    foreach (var t in tokensToRevoke)
                    {
                        if (t.RevokedAt == null)
                        {
                            t.RevokedAt = utcNow;
                            _context.Set<Shared.Models.RefreshToken>().Update(t);
                        }
                    }
                }
                else
                {
                    _context.Set<Shared.Models.RefreshToken>().RemoveRange(tokensToRevoke);
                }
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
