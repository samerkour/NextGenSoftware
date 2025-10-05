using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Abstractions.Messaging;
using Microsoft.AspNetCore.Identity;
using NextGen.Modules.Identity.Shared.Exceptions;
using NextGen.Modules.Identity.Shared.Models;
using NextGen.Modules.Identity.Users.Features.UserTwoFactorEnable.Events.Integration;

namespace NextGen.Modules.Identity.Users.Features.UserTwoFactorEnable;
internal class UpdateTwoFactorHandler : ICommandHandler<UpdateTwoFactorCommand, bool>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<UpdateTwoFactorHandler> _logger;
    private readonly IBus _bus;

    public UpdateTwoFactorHandler(
        UserManager<ApplicationUser> userManager,
        ILogger<UpdateTwoFactorHandler> logger,
        IBus bus)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    }

    public async Task<bool> Handle(UpdateTwoFactorCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
            throw new UserNotFoundException(request.UserId);

        // If already same state, return true
        if (user.TwoFactorEnabled == request.IsTwoFactorEnabled)
            return true;

        user.TwoFactorEnabled = request.IsTwoFactorEnabled;
        user.TwoFactorEnabledOn = request.IsTwoFactorEnabled ? DateTime.UtcNow : null;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            _logger.LogWarning("Failed to update TwoFactorEnabled for user {UserId}", request.UserId);
            throw new UpdateTwoFactorFailedException(request.UserId, request.IsTwoFactorEnabled);
        }

        // Publish domain event
        var twoFactorUpdated = new UserTwoFactorUpdated(user.Id, request.IsTwoFactorEnabled, user.TwoFactorEnabledOn);
        await _bus.PublishAsync(twoFactorUpdated, null, cancellationToken);

        _logger.LogInformation("TwoFactor updated for user {UserId} -> {Enabled}", user.Id, request.IsTwoFactorEnabled);

        return true;
    }
}
