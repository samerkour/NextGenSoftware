using BuildingBlocks.Abstractions.CQRS.Command;

namespace NextGen.Modules.Identity.Users.Features.RegisteringUser;

public record RegisterUserCommand(
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    string Password,
    string ConfirmPassword,
    string? MiddleName = null,
    DateTime? DateOfBirth = null,
    string? PlaceOfBirth = null,
    string? ProfileImagePath = null,
    string? Country = null,
    string? City = null,
    string? State = null,
    string? Address = null,
    string? PostalCode = null,
    DateTime? DeletedOn = null
) : ITxCreateCommand<RegisterUserResponse>
{
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public Guid Id { get; init; } = Guid.NewGuid();
    public IList<string> Roles { get; init; } = new List<string> { Constants.Role.User };
}
