namespace NextGen.Modules.Identity.ClaimGroups.Features.GetClaimsByGroup
{
    public class GetClaimsByGroupRequest
    {
        public Guid GroupId { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 50;
    }
}
