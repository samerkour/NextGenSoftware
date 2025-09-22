using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Abstractions.Messaging;
using BuildingBlocks.Core.Exception;
using BuildingBlocks.Core.Exception.Types;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NextGen.Modules.Identity.Shared.Exceptions;
using NextGen.Modules.Identity.Shared.Models;
using NextGen.Modules.Identity.Users.Features.UpdatingUserLockout.Events.Integration;

namespace NextGen.Modules.Identity.Users.Features.UpdatingUserLockout;

internal class UpdateUserLockoutHandler : ICommandHandler<UpdateUserLockoutCommand>
{
    private readonly IBus _bus;
    private readonly ILogger<UpdateUserLockoutHandler> _logger;
    private readonly UserManager<ApplicationUser> _userManager;

    public UpdateUserLockoutHandler(
        IBus bus,
        UserManager<ApplicationUser> userManager,
        ILogger<UpdateUserLockoutHandler> logger)
    {
        _bus = bus;
        _logger = logger;
        _userManager = Guard.Against.Null(userManager, nameof(userManager));
    }

    public async Task<Unit> Handle(UpdateUserLockoutCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        Guard.Against.Null(user, new UserNotFoundException(request.UserId));

        // only lockout non-admins
        if (await _userManager.IsInRoleAsync(user, Constants.Role.SecurityAdmin) || await _userManager.IsInRoleAsync(user, Constants.Role.Admin))
        {
            throw new UserCannotBeLockedOutException(user.UserName);
        }

        if (!user.LockoutEnabled)
        {
            throw new UserCannotBeLockedOutException(user.UserName);
        }

        if (request.LockoutEnd.HasValue)
        {
            user.LockoutEnd = request.LockoutEnd;
            user.LockoutEnabledOn = DateTime.UtcNow;
        }
        else
        {
            // unlock if LockoutEnd is cleared
            user.LockoutEnd = null;
            user.LockoutEnabledOn = null;
        }

        await _userManager.UpdateAsync(user);

        await _bus.PublishAsync(
            new UserLockoutUpdated(request.UserId, request.LockoutEnd),
            null,
            cancellationToken);

        _logger.LogInformation(
            "Updated lockout for user with ID: '{UserId}', LockoutEnd = '{LockoutEnd}', LockoutEnabledOn = '{LockoutEnabledOn}'",
            user.Id,
            user.LockoutEnd,
            user.LockoutEnabledOn);

        return Unit.Value;
    }
}
