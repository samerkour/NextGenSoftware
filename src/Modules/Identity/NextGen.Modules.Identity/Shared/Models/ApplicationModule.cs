using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.Modules.Identity.Shared.Models;
public class ApplicationModule
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }

    // Navigation
    public virtual ICollection<RoleGroup> RoleGroups { get; set; } = new HashSet<RoleGroup>();
}
