using System.Globalization;
using Microsoft.AspNetCore.Identity;

namespace NextGen.Modules.Identity.Shared.Models;

public class Role : IdentityRole<Guid>
{
    public Guid? RoleGroupId { get; set; }
    public virtual RoleGroup? RoleGroup { get; set; }

    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = new HashSet<ApplicationUserRole>();

    public virtual ICollection<RoleClaimGroup> RoleClaimGroups { get; set; } = new HashSet<RoleClaimGroup>();

    public virtual ICollection<RoleClaim> RoleClaims { get; set; } = new HashSet<RoleClaim>();

    // Factory roles
    public static Role SecurityAdmin => new()
    {
        Name = Constants.Role.SecurityAdmin,
        NormalizedName = nameof(SecurityAdmin).ToUpper(CultureInfo.InvariantCulture)
    };

    public static Role Admin => new()
    {
        Name = Constants.Role.Admin,
        NormalizedName = nameof(Admin).ToUpper(CultureInfo.InvariantCulture)
    };

    public static Role User => new()
    {
        Name = Constants.Role.User,
        NormalizedName = nameof(User).ToUpper(CultureInfo.InvariantCulture)
    };
}
