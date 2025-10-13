using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.Modules.Identity.Shared.Models;
public class RoleGroup
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }

    public Guid ModuleId { get; set; }
    public virtual ApplicationModule Module { get; set; } = default!;

    // Navigation
    public virtual ICollection<Role> Roles { get; set; } = new HashSet<Role>();
}
