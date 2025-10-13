using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.Modules.Identity.Shared.Models;
public class ApplicationClaim
{
    public Guid Id { get; set; }
    public string Type { get; set; } = default!;
    public string Value { get; set; } = default!;

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? DeletedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }

    // Navigation
    public virtual ICollection<ClaimGroupClaim> ClaimGroupClaims { get; set; } = new HashSet<ClaimGroupClaim>();
    public virtual ICollection<RoleClaim> RoleClaims { get; set; } = new HashSet<RoleClaim>();
}
