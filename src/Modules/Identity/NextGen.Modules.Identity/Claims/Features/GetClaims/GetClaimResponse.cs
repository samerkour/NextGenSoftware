namespace NextGen.Modules.Identity.Claims.Features.GetClaims
{
    public class Response
    {
        public Guid Id { get; set; }
        public string Type { get; set; } = default!;
        public string Value { get; set; } = default!;
        public Guid? ClaimGroupId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
