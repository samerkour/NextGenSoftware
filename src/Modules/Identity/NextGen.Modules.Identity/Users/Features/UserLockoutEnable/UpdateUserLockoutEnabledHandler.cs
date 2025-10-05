using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.Messaging;
using Microsoft.AspNetCore.Identity;
using NextGen.Modules.Identity.Shared.Models;
using NextGen.Modules.Identity.Users.Features.UserLockoutEnable.Events.Integration;

namespace NextGen.Modules.Identity.Users.Features.UserLockoutEnable;
internal class UpdateUserLockoutEnabledHandler : IRequestHandler<UpdateUserLockoutEnabledCommand, bool>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IBus _bus;

    public UpdateUserLockoutEnabledHandler(UserManager<ApplicationUser> userManager, IBus bus)
    {
        _userManager = Guard.Against.Null(userManager, nameof(userManager));
        _bus = Guard.Against.Null(bus, nameof(bus));
    }

    public async Task<bool> Handle(UpdateUserLockoutEnabledCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
            return false;

        user.LockoutEnabled = request.IsLockoutEnabled;
        user.LockoutEnabledOn = request.IsLockoutEnabled ? DateTime.UtcNow : null;
        user.LockoutEnd = /*request.LockoutEnabled ? DateTime.UtcNow :*/ null;

        var result = await _userManager.UpdateAsync(user);

        // Publish domain event
        await _bus.PublishAsync(new UserLockoutEnabledUpdated(user.Id, user.LockoutEnabled, user.LockoutEnabledOn), null, cancellationToken);

        return result.Succeeded;
    }
}
