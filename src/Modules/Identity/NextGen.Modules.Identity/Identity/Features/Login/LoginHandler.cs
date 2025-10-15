using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Abstractions.CQRS.Query;
using BuildingBlocks.Core.Exception;
using BuildingBlocks.Security.Jwt;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using NextGen.Modules.Identity.Shared.Extensions;
using NextGen.Modules.Identity.Shared.Models;

namespace NextGen.Modules.Identity.Identity.Features.Login;

public class LoginHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IMemoryCache _cache;
    private readonly ILogger<LoginHandler> _logger;
    private readonly AccountLockoutOptions _lockoutOptions;
    private readonly ICommandProcessor _commandProcessor;
    private readonly JwtOptions _jwtOptions;
    private readonly IStringLocalizer<LoginHandler> _localizer;

    public LoginHandler(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IStringLocalizer<LoginHandler> localizer,
        IMemoryCache cache,
        ILogger<LoginHandler> logger,
        IOptions<AccountLockoutOptions> lockoutOptions,
        ICommandProcessor commandProcessor,
        IOptions<JwtOptions> jwtOptions)
    {
        _userManager = Guard.Against.Null(userManager, nameof(userManager));
        _signInManager = Guard.Against.Null(signInManager, nameof(signInManager));
        _cache = Guard.Against.Null(cache, nameof(cache));
        _logger = Guard.Against.Null(logger, nameof(logger));
        _lockoutOptions = lockoutOptions?.Value ?? new AccountLockoutOptions();
        _commandProcessor = Guard.Against.Null(commandProcessor, nameof(commandProcessor));
        _jwtOptions = jwtOptions?.Value ?? throw new ArgumentNullException(nameof(jwtOptions));
        _localizer = localizer;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        // First, verify the CAPTCHA
        if (!_cache.TryGetValue(request.Captcha.ToLowerInvariant(), out string cachedCaptchaCode))
        {
            _logger.LogWarning("Login attempt failed for {UserNameOrEmail}: Captcha is not found", request.UserNameOrEmail);
            throw new BadHttpRequestException(_localizer["CaptchaNotFound"]);
        }

        if (!string.Equals(request.Captcha, cachedCaptchaCode, StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogWarning("Login attempt failed for {UserNameOrEmail}: Captcha mismatch", request.UserNameOrEmail);
            _cache.Remove(request.Captcha.ToLowerInvariant());
            throw new BadHttpRequestException(_localizer["CaptchaInvalid"]);
        }

        // Remove the captcha from cache
        _cache.Remove(request.Captcha.ToLowerInvariant());

        var identityUser = await _userManager.FindByNameAsync(request.UserNameOrEmail) ??
                           await _userManager.FindByEmailAsync(request.UserNameOrEmail);

        if (identityUser == null)
        {
            _logger.LogWarning("Login attempt failed for {UserNameOrEmail}: User is not found", request.UserNameOrEmail);
            // keep message generic to avoid enumeration
            throw new BadHttpRequestException(_localizer["UserNotFound"]);
        }

        // Check password without automatic lockout (we will handle lockout using options)
        var signInResult = await _signInManager.CheckPasswordSignInAsync(identityUser, request.Password, lockoutOnFailure: false);

        if (!signInResult.Succeeded)
        {
            _logger.LogWarning(
                "Login attempt failed for {UserNameOrEmail}: {Reason}",
                request.UserNameOrEmail,
                signInResult.IsNotAllowed ? "Not allowed" :
                signInResult.IsLockedOut ? "Locked out" :
                signInResult.RequiresTwoFactor ? "Requires 2FA" : "Invalid password");

            if (signInResult.IsNotAllowed && !await _userManager.IsEmailConfirmedAsync(identityUser))
                throw new BadHttpRequestException(_localizer["UserEmailNotConfirmed"]);

            if (signInResult.IsNotAllowed && !await _userManager.IsPhoneNumberConfirmedAsync(identityUser))
                throw new BadHttpRequestException(_localizer["UserPhoneNotConfirmed"]);

            if (signInResult.IsLockedOut)
                throw new BadHttpRequestException(_localizer["LockedOut"]);

            if (signInResult.RequiresTwoFactor)
                throw new BadHttpRequestException(_localizer["RequiresTwoFactor"]);

            // Invalid password path:
            // If lockout disabled via config (MaxFailedAccessAttempts <= 0) skip locking
            if (_lockoutOptions.MaxFailedAccessAttempts <= 0)
            {
                // Still increment the AccessFailedCount for metrics if desired, but do not lock
                await _userManager.AccessFailedAsync(identityUser);
                throw new BadHttpRequestException(_localizer["InvalidPassword"]);
            }

            // increment failed attempts
            identityUser.AccessFailedCount += 1;
            var updateResult = await _userManager.UpdateAsync(identityUser);

            _logger.LogInformation("User {UserNameOrEmail} failed login attempt {FailedCount}", request.UserNameOrEmail, identityUser.AccessFailedCount);

            if (identityUser.AccessFailedCount >= _lockoutOptions.MaxFailedAccessAttempts)
            {
                // ensure lockout enabled on the user
                if (!identityUser.LockoutEnabled)
                {
                    await _userManager.SetLockoutEnabledAsync(identityUser, true);
                }

                // set lockout end
                var lockoutEnd = DateTimeOffset.UtcNow.AddMinutes(_lockoutOptions.LockoutDurationMinutes);
                await _userManager.SetLockoutEndDateAsync(identityUser, lockoutEnd);

                _logger.LogWarning(
                    "User {UserNameOrEmail} locked out until {LockoutEnd} after {FailedCount} failed attempts",
                    request.UserNameOrEmail, lockoutEnd, identityUser.AccessFailedCount);

                throw new BadHttpRequestException(_localizer["UserLockedOutMultipleAttempts"]);
            }

            throw new BadHttpRequestException(_localizer["InvalidPassword"]);
        }

        // Successful login: optionally reset failed attempts
        if (_lockoutOptions.ResetFailedCountOnSuccess)
        {
            await _userManager.ResetAccessFailedCountAsync(identityUser);
        }

        // Generate refresh token and access token as before
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
