using System.Security.Claims;
using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Security.Jwt;
using NextGen.Modules.Identity.Identity.Exceptions;
using NextGen.Modules.Identity.Identity.Features.GenerateJwtToken;
using NextGen.Modules.Identity.Identity.Features.GenerateRefreshToken;
using NextGen.Modules.Identity.Shared.Exceptions;
using NextGen.Modules.Identity.Shared.Models;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace NextGen.Modules.Identity.Identity.Features.RefreshingToken;

internal class RefreshTokenHandler : ICommandHandler<RefreshTokenCommand, RefreshTokenResponse>
{
    private readonly ICommandProcessor _commandProcessor;
    private readonly IJwtService _jwtService;
    private readonly UserManager<ApplicationUser> _userManager;

    public RefreshTokenHandler(
        IJwtService jwtService,
        UserManager<ApplicationUser> userManager,
        ICommandProcessor commandProcessor)
    {
        _jwtService = jwtService;
        _userManager = userManager;
        _commandProcessor = commandProcessor;
    }

    public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(RefreshTokenCommand));

        // invalid token/signing key was passed and we can't extract user claims
        var userClaimsPrincipal = _jwtService.GetPrincipalFromToken(request.AccessTokenData);

        if (userClaimsPrincipal is null)
            throw new InvalidTokenException(userClaimsPrincipal);

        var userId = userClaimsPrincipal.FindFirstValue(JwtRegisteredClaimNames.NameId);

        var identityUser = await _userManager.FindByIdAsync(userId);

        if (identityUser == null)
            throw new UserNotFoundException(userId);

        var refreshToken =
            (await _commandProcessor.SendAsync(
                new GenerateRefreshToken.GenerateRefreshToken { UserId = identityUser.Id, Token = request.RefreshTokenData },
                cancellationToken)).RefreshToken;

        var accessToken =
            await _commandProcessor.SendAsync(
                new GenerateJwtToken.GenerateJwtToken(identityUser, refreshToken.Token), cancellationToken);

        return new RefreshTokenResponse(identityUser, accessToken, refreshToken.Token);
    }
}
