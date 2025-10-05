namespace NextGen.Modules.Identity.Claims.Features.CreateClaim
{
    public class CreateClaimRequest
    {
        public string Type { get; set; } = default!;
        public string Value { get; set; } = default!;
        public Guid ClaimGroupId { get; set; }
    }
}
