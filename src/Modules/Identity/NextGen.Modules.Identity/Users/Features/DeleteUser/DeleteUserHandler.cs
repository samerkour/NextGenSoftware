using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Abstractions.Messaging;
using Microsoft.AspNetCore.Identity;
using NextGen.Modules.Identity.Shared.Models;
using NextGen.Modules.Identity.Users.Features.DeleteUser.Events.Integration;

namespace NextGen.Modules.Identity.Users.Features.DeleteUser;

internal class DeleteUserHandler : ICommandHandler<DeleteUserCommand, bool>
{
    private readonly IBus _bus;
    private readonly ILogger<DeleteUserHandler> _logger;
    private readonly UserManager<ApplicationUser> _userManager;

    public DeleteUserHandler(
        IBus bus,
        UserManager<ApplicationUser> userManager,
        ILogger<DeleteUserHandler> logger)
    {
        _bus = bus;
        _logger = logger;
        _userManager = Guard.Against.Null(userManager, nameof(userManager));
    }

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
            throw new UserNotFoundException(request.UserId);

        if (await _userManager.IsInRoleAsync(user, Constants.Role.SecurityAdmin) || await _userManager.IsInRoleAsync(user, Constants.Role.Admin))
        {
            throw new UserCannotBeDeletedException(request.UserId);
        }

        if (request.IsDeleted)
        {
            // Soft delete instead of hard delete
            user.DeletedOn = DateTime.UtcNow;
        }
        else
        {
            user.DeletedOn = null;
        }

        var result = await _userManager.UpdateAsync(user);

        var userDeleted = new UserDeleted(request.UserId, user.DeletedOn);
        await _bus.PublishAsync(userDeleted, null, cancellationToken);

        _logger.LogInformation("Deleted user with ID: '{UserId}' at {DeletedOn}", user.Id, user.DeletedOn);

        return result.Succeeded;
    }
}
