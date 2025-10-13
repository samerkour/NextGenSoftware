using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.Modules.Identity.Claims.Dtos;

public record ClaimDto
{
    public Guid Id { get; init; }
    public string Type { get; init; } = default!;
    public string Value { get; init; } = default!;
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public Guid? ClaimGroupId { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedOn { get; init; }
    public DateTime? DeletedOn { get; init; }
}
