using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextGen.Modules.Identity.Shared.Models;

public class ClaimGroup
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }

    // Navigation
    public virtual ICollection<ApplicationClaim> Claims { get; set; } = new HashSet<ApplicationClaim>();
    public virtual ICollection<ApplicationRole> Roles { get; set; } = new HashSet<ApplicationRole>();
}
