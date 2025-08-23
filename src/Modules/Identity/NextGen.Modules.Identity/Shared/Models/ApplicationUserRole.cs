using Microsoft.AspNetCore.Identity;

namespace NextGen.Modules.Identity.Shared.Models;

public class ApplicationUserRole : IdentityUserRole<Guid>
{
    public virtual ApplicationUser? User { get; set; }
    public virtual ApplicationRole? Role { get; set; }
}
