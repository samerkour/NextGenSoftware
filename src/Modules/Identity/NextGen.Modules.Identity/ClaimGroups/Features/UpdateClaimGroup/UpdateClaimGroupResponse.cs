namespace NextGen.Modules.Identity.ClaimGroups.Features.UpdateClaimGroup
{
    public class UpdateClaimGroupResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
