using Microsoft.AspNetCore.Identity;

namespace NextGen.Modules.Identity.Shared.Models;

public class ApplicationUser : IdentityUser<Guid>
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? MiddleName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? PlaceOfBirth { get; set; }
    public string? ProfileImagePath { get; set; }

    // Location / Address fields
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? Address { get; set; }
    public string? PostalCode { get; set; }
    public string? State { get; set; } // optional, if your system uses states/provinces

    public DateTime LastLoggedInAt { get; set; }
    public DateTime PasswordLastChangedOn { get; set; }
    public DateTime? TwoFactorEnabledOn { get; set; }
    public DateTime? LockoutEnabledOn { get; set; }

    // Identity navigations
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = default!;
    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = default!;

    public DateTime CreatedAt { get; set; }

    // Soft delete marker
    public DateTime? DeletedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
