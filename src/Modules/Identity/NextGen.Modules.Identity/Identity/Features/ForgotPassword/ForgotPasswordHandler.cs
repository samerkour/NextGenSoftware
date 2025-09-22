using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Abstractions.Messaging;
using global::NextGen.Modules.Identity.Shared.Models;
using Microsoft.AspNetCore.Identity;
using NextGen.Modules.Identity.Identity.Features.ForgotPassword.Events.Integration;
using NextGen.Modules.Identity.Shared.Exceptions;

namespace NextGen.Modules.Identity.Identity.Features.ForgotPassword;

internal class ForgotPasswordHandler : ICommandHandler<ForgotPasswordCommand>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IBus _bus;

    public ForgotPasswordHandler(UserManager<ApplicationUser> userManager, IBus bus)
    {
        _userManager = Guard.Against.Null(userManager, nameof(userManager));
        _bus = bus;
    }

    public async Task<Unit> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
            throw new UserNotFoundException(Guid.Empty); // email case, Guid.Empty signals lookup by email

        if (!(await _userManager.IsEmailConfirmedAsync(user)))
            throw new ForgotPasswordException("Email is not confirmed.");

        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

        // publish event so email service can handle sending reset link
        await _bus.PublishAsync(
            new UserForgotPassword(user.Id, user.Email!, resetToken),
            null,
            cancellationToken);

        return Unit.Value;
    }
}
