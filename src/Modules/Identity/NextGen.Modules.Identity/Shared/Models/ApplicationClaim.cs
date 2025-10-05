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
    public DateTime CreatedAt { get; set; }
    public DateTime? DeletedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }

    // FK to ClaimGroup
    public Guid ClaimGroupId { get; set; }
    public virtual ClaimGroup ClaimGroup { get; set; } = default!;
}
