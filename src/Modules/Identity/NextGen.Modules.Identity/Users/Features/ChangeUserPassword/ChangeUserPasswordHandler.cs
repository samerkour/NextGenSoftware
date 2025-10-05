using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Command;
using global::NextGen.Modules.Identity.Shared.Models;
using Microsoft.AspNetCore.Identity;

namespace NextGen.Modules.Identity.Users.Features.ChangeUserPassword;

internal class ChangeUserPasswordHandler : ICommandHandler<ChangeUserPasswordCommand>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ChangeUserPasswordHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = Guard.Against.Null(userManager, nameof(userManager));
    }

    public async Task<Unit> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
            throw new UserNotFoundException(request.UserId);

        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

        if (!result.Succeeded)
            throw new ChangeUserPasswordException(string.Join(',', result.Errors.Select(e => e.Description)));

        user.PasswordLastChangedOn = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        return Unit.Value;
    }
}
