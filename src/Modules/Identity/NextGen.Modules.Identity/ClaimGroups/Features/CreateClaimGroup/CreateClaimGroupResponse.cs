namespace NextGen.Modules.Identity.ClaimGroups.Features.CreateClaimGroup
{
    public class CreateClaimGroupResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
