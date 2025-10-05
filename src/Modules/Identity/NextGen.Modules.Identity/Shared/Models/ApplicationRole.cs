using System.Globalization;
using Microsoft.AspNetCore.Identity;

namespace NextGen.Modules.Identity.Shared.Models;

public class ApplicationRole : IdentityRole<Guid>
{
    public Guid? RoleGroupId { get; set; }
    public virtual RoleGroup? RoleGroup { get; set; }

    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = default!;
    public virtual ICollection<ClaimGroup> ClaimGroups { get; set; } = new HashSet<ClaimGroup>();

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
        NormalizedName = nameof(User).ToUpper(CultureInfo.InvariantCulture),
    };
}
