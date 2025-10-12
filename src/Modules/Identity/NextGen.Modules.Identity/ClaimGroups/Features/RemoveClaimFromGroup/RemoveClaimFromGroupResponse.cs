namespace NextGen.Modules.Identity.ClaimGroups.Features.RemoveClaimFromGroup
{
    public class RemoveClaimFromGroupResponse
    {
        public Guid GroupId { get; set; }
        public Guid ClaimId { get; set; }
        public bool Removed { get; set; }
    }
}
