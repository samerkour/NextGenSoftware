using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Abstractions.CQRS.Query;
using BuildingBlocks.Core.Exception;
using BuildingBlocks.Security.Jwt;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using NextGen.Modules.Identity.Shared.Models;

namespace NextGen.Modules.Identity.Identity.Features.Login;

internal class LoginHandler : ICommandHandler<LoginCommand, LoginResponse>
{
    private readonly ICommandProcessor _commandProcessor;
    private readonly IJwtService _jwtService;
    private readonly JwtOptions _jwtOptions;
    private readonly ILogger<LoginHandler> _logger;
    private readonly IQueryProcessor _queryProcessor;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IMemoryCache _cache;
    private readonly UserManager<ApplicationUser> _userManager;

    public LoginHandler(
        UserManager<ApplicationUser> userManager,
        ICommandProcessor commandProcessor,
        IQueryProcessor queryProcessor,
        IJwtService jwtService,
        IOptions<JwtOptions> jwtOptions,
        SignInManager<ApplicationUser> signInManager,
        IMemoryCache cache,
        ILogger<LoginHandler> logger)
    {
        _userManager = userManager;
        _commandProcessor = commandProcessor;
        _queryProcessor = queryProcessor;
        _jwtService = jwtService;
        _signInManager = signInManager;
        _cache = cache;
        _jwtOptions = jwtOptions.Value;
        _logger = logger;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(LoginCommand));

        // First, verify the CAPTCHA
        if (!_cache.TryGetValue(request.Captcha.ToLower(), out string cachedCaptchaCode))
        {
            _logger.LogWarning("Login attempt failed for {UserNameOrEmail}: Capatcha is not found", request.UserNameOrEmail);
            throw new LoginFailedException(request.UserNameOrEmail, "Capatcha is not found");
        }

        if (!string.Equals(request.Captcha.ToLower(), cachedCaptchaCode, StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogWarning("Login attempt failed for {UserNameOrEmail}: Capatcha is not found", request.UserNameOrEmail);
            throw new LoginFailedException(request.UserNameOrEmail, "Capatcha is not found");
        }

        // Remove the captcha from cache
        _cache.Remove(request.Captcha.ToLower());

        var identityUser = await _userManager.FindByNameAsync(request.UserNameOrEmail) ??
                    await _userManager.FindByEmailAsync(request.UserNameOrEmail);

        // Avoid specific exceptions until after password check
        if (identityUser == null)
        {
            // Use a generic error message for all login failures to prevent user enumeration, and log detailed errors internally.
            // A generic LoginFailedException with a message like “Login failed for username: {UserNameOrEmail}” prevents attackers from distinguishing between invalid usernames and passwords. Detailed logging helps with debugging without exposing sensitive details to clients.
            _logger.LogWarning("Login attempt failed for {UserNameOrEmail}: User is not found", request.UserNameOrEmail);
            throw new LoginFailedException(request.UserNameOrEmail, "User is not found");
        }

        var signinResult = await _signInManager.CheckPasswordSignInAsync(identityUser, request.Password, false);

        if (!signinResult.Succeeded)
        {
            _logger.LogWarning(
                "Login attempt failed for {UserNameOrEmail}: {Reason}",
                request.UserNameOrEmail,
                signinResult.IsNotAllowed ? "Not allowed" :
                signinResult.IsLockedOut ? "Locked out" :
                signinResult.RequiresTwoFactor ? "Requires 2FA" : "Invalid password");

            if (signinResult.IsNotAllowed && !await _userManager.IsEmailConfirmedAsync(identityUser))
                throw new LoginFailedException(request.UserNameOrEmail, "User email is not confirmed");
            if (signinResult.IsNotAllowed && !await _userManager.IsPhoneNumberConfirmedAsync(identityUser))
                throw new LoginFailedException(request.UserNameOrEmail, "User phone number is not confirmed");
            if (signinResult.IsLockedOut)
                throw new LoginFailedException(request.UserNameOrEmail, "User is locked out");
            if (signinResult.RequiresTwoFactor)
                throw new LoginFailedException(request.UserNameOrEmail, "tow factor Authentication is required");

            throw new LoginFailedException(request.UserNameOrEmail, "User not found");
        }

        var refreshToken = (await _commandProcessor.SendAsync(
            new GenerateRefreshToken.GenerateRefreshToken { UserId = identityUser.Id },
            cancellationToken)).RefreshToken;

        var accessToken = await _commandProcessor.SendAsync(
            new GenerateJwtToken.GenerateJwtToken(identityUser, refreshToken.Token),
            cancellationToken);

        _logger.LogInformation("User with ID: {ID} has been authenticated", identityUser.Id);

        return new LoginResponse(identityUser, "Bearer", _jwtOptions.TokenLifeTimeSecond == 0 ? 36000 : _jwtOptions.TokenLifeTimeSecond, accessToken, refreshToken.Token);
    }
}
