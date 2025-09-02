using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Abstractions.CQRS.Query;
using BuildingBlocks.Abstractions.Persistence;
using BuildingBlocks.Core.Exception;
using BuildingBlocks.Security.Jwt;
using FirebirdSql.Data.Services;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using NextGen.Modules.Identity.Identity.Exceptions;
using NextGen.Modules.Identity.Shared.Exceptions;
using NextGen.Modules.Identity.Shared.Models;
using SendGrid.Helpers.Mail;
using Spectre.Console;
using static IdentityModel.OidcConstants;

namespace NextGen.Modules.Identity.Identity.Features.Login;

public record LoginCommand(string UserNameOrEmail, string Password, bool Remember) :
    ICommand<LoginResponse>, ITxRequest;

internal class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator()
    {
        // Validations to prevent malformed inputs, aligns with security best practices, and reduces the risk of injection attacks(though ASP.NET Core Identity sanitizes inputs).
        RuleFor(x => x.UserNameOrEmail)
            .NotEmpty().WithMessage("UserNameOrEmail cannot be empty")
            .MaximumLength(256).WithMessage("UserNameOrEmail cannot exceed 256 characters")
            .When(x => x.UserNameOrEmail.Contains('@'))
            .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("Invalid email format");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password cannot be empty")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters")
            .MaximumLength(128).WithMessage("Password cannot exceed 128 characters")
            .Matches(@"[A-Za-z0-9!@#$%^&*]").WithMessage("Password must contain at least one letter, number, or special character");
    }
}

internal class LoginHandler : ICommandHandler<LoginCommand, LoginResponse>
{
    private readonly ICommandProcessor _commandProcessor;
    private readonly IJwtService _jwtService;
    private readonly JwtOptions _jwtOptions;
    private readonly ILogger<LoginHandler> _logger;
    private readonly IQueryProcessor _queryProcessor;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public LoginHandler(
        UserManager<ApplicationUser> userManager,
        ICommandProcessor commandProcessor,
        IQueryProcessor queryProcessor,
        IJwtService jwtService,
        IOptions<JwtOptions> jwtOptions,
        SignInManager<ApplicationUser> signInManager,
        ILogger<LoginHandler> logger)
    {
        _userManager = userManager;
        _commandProcessor = commandProcessor;
        _queryProcessor = queryProcessor;
        _jwtService = jwtService;
        _signInManager = signInManager;
        _jwtOptions = jwtOptions.Value;
        _logger = logger;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(LoginCommand));

        var identityUser = await _userManager.FindByNameAsync(request.UserNameOrEmail) ??
                           await _userManager.FindByEmailAsync(request.UserNameOrEmail);

        // Avoid specific exceptions until after password check
        if (identityUser == null)
        {
            // Use a generic error message for all login failures to prevent user enumeration, and log detailed errors internally.
            // A generic LoginFailedException with a message like “Login failed for username: {UserNameOrEmail}” prevents attackers from distinguishing between invalid usernames and passwords. Detailed logging helps with debugging without exposing sensitive details to clients.
            _logger.LogWarning("Login attempt failed for {UserNameOrEmail}: User not found", request.UserNameOrEmail);
            throw new LoginFailedException(request.UserNameOrEmail);
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
                throw new LoginFailedException(request.UserNameOrEmail);
            if (signinResult.IsNotAllowed && !await _userManager.IsPhoneNumberConfirmedAsync(identityUser))
                throw new LoginFailedException(request.UserNameOrEmail);
            if (signinResult.IsLockedOut)
                throw new LoginFailedException(request.UserNameOrEmail);
            if (signinResult.RequiresTwoFactor)
                throw new LoginFailedException(request.UserNameOrEmail);

            throw new LoginFailedException(request.UserNameOrEmail);
        }

        var refreshToken = (await _commandProcessor.SendAsync(
            new GenerateRefreshToken.GenerateRefreshToken { UserId = identityUser.Id },
            cancellationToken)).RefreshToken;

        var accessToken = await _commandProcessor.SendAsync(
            new GenerateJwtToken.GenerateJwtToken(identityUser, refreshToken.Token),
            cancellationToken);

        _logger.LogInformation("User with ID: {ID} has been authenticated", identityUser.Id);

        return new LoginResponse(identityUser, accessToken, refreshToken.Token);
    }
}
