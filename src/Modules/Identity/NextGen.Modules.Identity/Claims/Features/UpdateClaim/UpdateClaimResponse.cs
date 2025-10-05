namespace NextGen.Modules.Identity.Claims.Features.UpdateClaim;

public class UpdateClaimResponse
{
    public Guid Id { get; set; }
    public string Type { get; set; } = default!;
    public string Value { get; set; } = default!;
    public Guid? ClaimGroupId { get; set; }
}
