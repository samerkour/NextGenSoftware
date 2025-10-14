using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.Modules.Identity.Shared.Models;
public class RoleGroup
{
    public Guid Id { get; set; }
    public Guid ModuleId { get; set; }
    public ApplicationModule Module { get; set; } = default!;

    public string Name { get; set; } = default!;
    public string? Description { get; set; }

    public ICollection<RoleGroupRole> RoleGroupRoles { get; set; } = new List<RoleGroupRole>();
}

