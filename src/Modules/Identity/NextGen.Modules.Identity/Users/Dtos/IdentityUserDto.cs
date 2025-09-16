using NextGen.Modules.Identity.Shared.Models;

namespace NextGen.Modules.Identity.Users.Dtos;

public record IdentityUserDto
{
    public Guid Id { get; init; }
    public string UserName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public DateTime? LastLoggedInAt { get; init; }
    public IEnumerable<string>? RefreshTokens { get; init; }
    public IEnumerable<string>? Roles { get; init; }
    public UserState UserState { get; init; }
    public DateTime CreatedAt { get; init; }
}
