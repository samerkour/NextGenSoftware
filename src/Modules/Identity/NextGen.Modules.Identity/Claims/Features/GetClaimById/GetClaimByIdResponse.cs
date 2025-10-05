namespace NextGen.Modules.Identity.Claims.Features.GetClaimById
{
    public class GetClaimByIdResponse
    {
        public Guid Id { get; set; }
        public string Type { get; set; } = default!;
        public string Value { get; set; } = default!;
        public Guid? ClaimGroupId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
