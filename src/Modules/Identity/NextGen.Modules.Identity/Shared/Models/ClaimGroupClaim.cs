using NextGen.Modules.Identity.Shared.Models;

public class ClaimGroupClaim
{
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }

    public Guid ClaimGroupId { get; set; }
    public ClaimGroup ClaimGroup { get; set; } = null!;

    public Guid ClaimId { get; set; }
    public ApplicationClaim Claim { get; set; } = null!;
}
