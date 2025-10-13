using NextGen.Modules.Identity.Shared.Models;

public class ClaimGroupClaim
{
    public Guid ClaimGroupId { get; set; }
    public ClaimGroup ClaimGroup { get; set; } = null!;

    public Guid ClaimId { get; set; }
    public ApplicationClaim Claim { get; set; } = null!;
}
