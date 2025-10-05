using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Abstractions.Messaging;
using Microsoft.AspNetCore.Identity;
using NextGen.Modules.Identity.Identity.Features.ResetPassword.Events.Integration;
using NextGen.Modules.Identity.Shared.Exceptions;
using NextGen.Modules.Identity.Shared.Models;

namespace NextGen.Modules.Identity.Identity.Features.ResetPassword;

internal class ResetPasswordHandler : ICommandHandler<ResetPasswordCommand>
{
    public IBus _bus { get; }
    private readonly UserManager<ApplicationUser> _userManager;

    public ResetPasswordHandler(UserManager<ApplicationUser> userManager, IBus bus)
    {
        _userManager = Guard.Against.Null(userManager, nameof(userManager));
        _bus = bus;
    }

    public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
            throw new UserNotFoundException(request.UserId);

        var resetResult = await _userManager.ResetPasswordAsync(user, request.ResetToken, request.NewPassword);

        if (!resetResult.Succeeded)
            throw new ResetPasswordException(string.Join(',', resetResult.Errors.Select(e => e.Description)));

        user.PasswordLastChangedOn = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        await _bus.PublishAsync(new UserPasswordReset(user.Id, DateTime.UtcNow), null, cancellationToken);

        return Unit.Value;
    }
}
