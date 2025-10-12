using System;
using System.Collections.Generic;
using NextGen.Modules.Identity.Shared.Models;

namespace NextGen.Modules.Identity.Shared.Models;
public class ClaimGroup
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedOn { get; set; }

    // Claims inside this group
    public virtual ICollection<ApplicationClaim> Claims { get; set; } = new HashSet<ApplicationClaim>();

    // ✅ Removed direct Roles navigation — use RoleClaimGroups instead
    public virtual ICollection<RoleClaimGroup> RoleClaimGroups { get; set; } = new HashSet<RoleClaimGroup>();
}
