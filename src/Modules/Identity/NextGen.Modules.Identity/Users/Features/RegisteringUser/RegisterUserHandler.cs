using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Abstractions.Messaging;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using NextGen.Modules.Identity.Shared.Models;
using NextGen.Modules.Identity.Users.Dtos;
using NextGen.Modules.Identity.Users.Exceptions;
using NextGen.Modules.Identity.Users.Features.RegisteringUser.Events.Integration;
using NextGen.Modules.Identity.Users.Features.UpdatingUser;
using UserState = NextGen.Modules.Identity.Shared.Models.UserState;

namespace NextGen.Modules.Identity.Users.Features.RegisteringUser;

// using transaction script instead of using domain business logic here
// https://www.youtube.com/watch?v=PrJIMTZsbDw
internal class RegisterUserHandler : ICommandHandler<RegisterUserCommand, RegisterUserResponse>
{
    private readonly IBus _bus;

    private readonly UserManager<ApplicationUser> _userManager;

    public RegisterUserHandler(UserManager<ApplicationUser> userManager, IBus bus)
    {
        _bus = bus;
        _userManager = Guard.Against.Null(userManager, nameof(userManager));
    }

    public async Task<RegisterUserResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        // Check for existing email
        var existingEmailUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingEmailUser != null)
            throw new DuplicateUserException("Email", request.Email);

        // Check for existing username
        var existingUserNameUser = await _userManager.FindByNameAsync(request.UserName);
        if (existingUserNameUser != null)
            throw new DuplicateUserException("UserName", request.UserName);

        // Create user
        var applicationUser = new ApplicationUser
        {
            Id = request.Id,
            FirstName = request.FirstName,
            LastName = request.LastName,
            MiddleName = request.MiddleName,
            UserName = request.UserName,
            Email = request.Email,
            DateOfBirth = request.DateOfBirth,
            PlaceOfBirth = request.PlaceOfBirth,
            ProfileImagePath = request.ProfileImagePath,
            Country = request.Country,
            City = request.City,
            State = request.State,
            Address = request.Address,
            PostalCode = request.PostalCode,
            DeletedOn = request.DeletedOn,
            CreatedAt = request.CreatedAt,
            PasswordLastChangedOn = DateTime.UtcNow
        };

        var identityResult = await _userManager.CreateAsync(applicationUser, request.Password);
        if (!identityResult.Succeeded)
            throw new RegisterIdentityUserException(string.Join(',', identityResult.Errors.Select(e => e.Description)));

        var roleResult = await _userManager.AddToRolesAsync(
            applicationUser,
            request.Roles ?? new List<string> { Constants.Role.User });

        if (!roleResult.Succeeded)
            throw new RegisterIdentityUserException(string.Join(',', roleResult.Errors.Select(e => e.Description)));

        await _bus.PublishAsync(
            new UserRegistered(
                applicationUser.Id,
                applicationUser.Email,
                applicationUser.UserName,
                applicationUser.FirstName,
                applicationUser.LastName,
                request.Roles),
            null,
            cancellationToken);

        return new RegisterUserResponse(new IdentityUserDto
        {
            Id = applicationUser.Id,
            Email = applicationUser.Email,
            UserName = applicationUser.UserName,
            FirstName = applicationUser.FirstName,
            LastName = applicationUser.LastName,
            Roles = request.Roles,
            CreatedAt = request.CreatedAt,
            DeletedOn = request.DeletedOn,
            MiddleName = applicationUser?.MiddleName,
            DateOfBirth = applicationUser?.DateOfBirth,
            PlaceOfBirth = applicationUser?.PlaceOfBirth,
            ProfileImagePath = applicationUser?.ProfileImagePath,
            Country = applicationUser?.Country,
            City = applicationUser?.City,
            State = applicationUser?.State,
            Address = applicationUser?.Address,
            LockoutEnabled = applicationUser?.LockoutEnabled ?? false,
            LockoutEnd = applicationUser?.LockoutEnd,
            IsDeleted = applicationUser?.DeletedOn.HasValue == true ? true : false,
            IsLocked = applicationUser?.LockoutEnd.HasValue == true ? true : false,
            IsTwoFactorEnabled = applicationUser?.TwoFactorEnabled ?? false,
            PostalCode = applicationUser?.PostalCode
        });
    }

}
