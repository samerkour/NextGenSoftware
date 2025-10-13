namespace NextGen.Modules.Identity.Claims.Features.UpdateClaim;

public class UpdateClaimRequest
{
    public string Type { get; set; } = default!;
    public string Value { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public Guid ClaimGroupId { get; set; }
}
