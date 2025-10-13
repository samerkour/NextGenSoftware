using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace NextGen.Modules.Identity.Shared.Models;

public class RoleClaimGroup
{
    public Guid RoleId { get; set; }
    public Guid ClaimGroupId { get; set; }

    public virtual Role Role { get; set; } = default!;
    public virtual ClaimGroup ClaimGroup { get; set; } = default!;
}
