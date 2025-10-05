using NextGen.Modules.Identity.Shared.Models;

namespace NextGen.Modules.Identity.Users.Dtos;

public record IdentityUserDto
{
    public Guid Id { get; init; }
    public string UserName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string? MiddleName { get; init; }
    public DateTime? DateOfBirth { get; init; }
    public string? PlaceOfBirth { get; init; }
    public string? ProfileImagePath { get; init; }

    // Location / Address
    public string? Country { get; init; }
    public string? City { get; init; }
    public string? State { get; init; }
    public string? Address { get; init; }
    public string? PostalCode { get; init; }

    public DateTime? LastLoggedInAt { get; init; }
    public DateTime PasswordLastChangedOn { get; init; }
    public DateTime? TwoFactorEnabledOn { get; init; }
    public bool LockoutEnabled { get; init; }
    public DateTime? LockoutEnabledOn { get; init; }
    public DateTimeOffset? LockoutEnd { get; init; }

    public IEnumerable<string>? Roles { get; init; }
    public DateTime? DeletedOn { get; init; }
    public bool IsDeleted { get; init; }
    public bool IsLocked { get; init; }
    public bool IsTwoFactorEnabled { get; init; }
    public DateTime CreatedAt { get; init; }
}
