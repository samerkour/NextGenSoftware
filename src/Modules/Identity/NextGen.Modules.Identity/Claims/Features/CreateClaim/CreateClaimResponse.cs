namespace NextGen.Modules.Identity.Claims.Features.CreateClaim
{
    public class CreateClaimResponse
    {
        public Guid Id { get; set; }
        public string Type { get; set; } = default!;
        public string Value { get; set; } = default!;
        public Guid ClaimGroupId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public DateTime? DeletedOn { get; set; }

    }
}
