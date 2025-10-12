using System.Globalization;
using Microsoft.AspNetCore.Identity;

namespace NextGen.Modules.Identity.Shared.Models;

public class ApplicationRole : IdentityRole<Guid>
{
    public Guid? RoleGroupId { get; set; }
    public virtual RoleGroup? RoleGroup { get; set; }

    // User assignments
    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = new HashSet<ApplicationUserRole>();

    // Removed direct ClaimGroups navigation — use RoleClaimGroups instead
    public virtual ICollection<RoleClaimGroup> RoleClaimGroups { get; set; } = new HashSet<RoleClaimGroup>();

    // Factory roles
    public static ApplicationRole SecurityAdmin => new()
    {
        Name = Constants.Role.SecurityAdmin,
        NormalizedName = nameof(SecurityAdmin).ToUpper(CultureInfo.InvariantCulture)
    };

    public static ApplicationRole Admin => new()
    {
        Name = Constants.Role.Admin,
        NormalizedName = nameof(Admin).ToUpper(CultureInfo.InvariantCulture)
    };

    public static ApplicationRole User => new()
    {
        Name = Constants.Role.User,
        NormalizedName = nameof(User).ToUpper(CultureInfo.InvariantCulture)
    };
}
