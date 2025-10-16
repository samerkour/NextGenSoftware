using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.Modules.Identity.Roles.Features.CreateRole
{
    public class CreateRoleRequest
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }
}
