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
    IList<string>? Roles = null
) : ITxCreateCommand<RegisterUserResponse>
{
    public DateTime CreatedAt { get; init; } = DateTime.Now;
    public Guid Id { get; init; } = Guid.NewGuid();
}
