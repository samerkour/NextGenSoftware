using Microsoft.AspNetCore.Identity;

namespace NextGen.Modules.Identity.Shared.Models;

public class ApplicationUserRole : IdentityUserRole<Guid>
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }

    public virtual ApplicationUser? User { get; set; } = default!;
    public virtual Role? Role { get; set; } = default!;
}
