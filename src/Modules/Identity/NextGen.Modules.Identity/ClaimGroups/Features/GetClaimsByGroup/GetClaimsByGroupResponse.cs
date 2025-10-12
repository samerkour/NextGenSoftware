namespace NextGen.Modules.Identity.ClaimGroups.Features.GetClaimsByGroup
{
    public class GetClaimsByGroupResponse
    {
        public Guid ClaimId { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}
