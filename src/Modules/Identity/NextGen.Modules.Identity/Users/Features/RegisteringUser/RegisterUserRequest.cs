namespace NextGen.Modules.Identity.Users.Features.RegisteringUser;

public record RegisterUserRequest(
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
    string? PostalCode = null
)
{
}
