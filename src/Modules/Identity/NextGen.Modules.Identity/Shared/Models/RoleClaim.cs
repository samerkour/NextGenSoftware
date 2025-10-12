using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace NextGen.Modules.Identity.Shared.Models;

public class RoleClaim
{
    public Guid RoleId { get; set; }
    public Guid ClaimId { get; set; }

    public ApplicationRole Role { get; set; } = default!;
    public ApplicationClaim Claim { get; set; } = default!;
}
