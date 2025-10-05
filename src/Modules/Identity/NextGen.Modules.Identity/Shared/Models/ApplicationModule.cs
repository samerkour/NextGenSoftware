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

    // Navigation
    public virtual ICollection<RoleGroup> RoleGroups { get; set; } = new HashSet<RoleGroup>();
}
