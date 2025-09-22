using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Core.Exception.Types;
using Microsoft.AspNetCore.Identity;
using NextGen.Modules.Identity.Shared.Models;
using NextGen.Modules.Identity.Users.Dtos;

namespace NextGen.Modules.Identity.Users.Features.UpdatingUser;

internal class UpdateUserHandler : ICommandHandler<UpdateUserCommand, UpdateUserResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UpdateUserHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = Guard.Against.Null(userManager, nameof(userManager));
    }

    public async Task<UpdateUserResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());
        if (user == null)
            throw new UserNotFoundException(request.Id);

        // update properties
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.MiddleName = request.MiddleName;
        user.UserName = request.UserName;
        user.Email = request.Email;
        user.DateOfBirth = request.DateOfBirth;
        user.PlaceOfBirth = request.PlaceOfBirth;
        user.ProfileImagePath = request.ProfileImagePath;
        user.Country = request.Country;
        user.City = request.City;
        user.State = request.State;
        user.Address = request.Address;
        user.PostalCode = request.PostalCode;

        var identityResult = await _userManager.UpdateAsync(user);
        if (!identityResult.Succeeded)
            throw new UpdateIdentityUserException(user.Id, string.Join(',', identityResult.Errors.Select(e => e.Description)));

        if (request.Roles is { Count: > 0 })
        {
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRolesAsync(user, request.Roles);
        }

        return new UpdateUserResponse(new IdentityUserDto
        {
            Id = user.Id,
            Email = user.Email,
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Roles = request.Roles ?? new List<string>(),
            RefreshTokens = user.RefreshTokens?.Where(x => !x.IsRevoked).Select(x => x.Token),
            CreatedAt = user.CreatedAt,
            DeletedOn = user.DeletedOn,
            MiddleName = user.MiddleName,
            DateOfBirth = user.DateOfBirth,
            PlaceOfBirth = user.PlaceOfBirth,
            ProfileImagePath = user.ProfileImagePath,
            Country = user.Country,
            City = user.City,
            State = user.State,
            Address = user.Address,
            PostalCode = user.PostalCode
        });
    }
}
