using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Core.Exception.Types;
using NextGen.Modules.Identity.Identity.Features.VerifyEmail.Exceptions;
using NextGen.Modules.Identity.Shared.Data;
using NextGen.Modules.Identity.Shared.Exceptions;
using NextGen.Modules.Identity.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace NextGen.Modules.Identity.Identity.Features.VerifyEmail;

internal class VerifyEmailHandler : ICommandHandler<VerifyEmailCommand>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IdentityContext _dbContext;
    private readonly ILogger<VerifyEmailHandler> _logger;

    public VerifyEmailHandler(
        UserManager<ApplicationUser> userManager,
        IdentityContext dbContext,
        ILogger<VerifyEmailHandler> logger)
    {
        _userManager = userManager;
        _dbContext = dbContext;
        _logger = logger;
    }


    public async Task<Unit> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(VerifyEmailCommand));

        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            throw new UserNotFoundException(request.Email);
        }

        if (user.EmailConfirmed)
        {
            throw new EmailAlreadyVerifiedException(user.Email);
        }

        var emailVerificationCode = await _dbContext.Set<EmailVerificationCode>()
            .Where(x => x.Email == request.Email && x.Code == request.Code && x.UsedAt == null)
            .SingleOrDefaultAsync(cancellationToken: cancellationToken);

        if (emailVerificationCode == null)
        {
            throw new BadRequestException("Either email or code is incorrect.");
        }

        if (DateTime.Now > emailVerificationCode.SentAt.AddMinutes(5))
        {
            throw new BadRequestException("The code is expired.");
        }

        user.EmailConfirmed = true;
        await _userManager.UpdateAsync(user);

        emailVerificationCode.UsedAt = DateTime.Now;
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Email verified successfully for userId:{UserId}", user.Id);

        return Unit.Value;
    }
}
