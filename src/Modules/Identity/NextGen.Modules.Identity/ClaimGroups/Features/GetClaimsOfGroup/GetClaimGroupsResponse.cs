namespace NextGen.Modules.Identity.ClaimGroups.Features.GetClaimGroups
{
    public class GetClaimGroupsResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty; // Null-safe
        public string Description { get; set; } = string.Empty; // Null-safe
        public DateTime CreatedAt { get; set; } // مقدار DB
        public DateTime UpdatedOn { get; set; } // مقدار DB
    }
}
