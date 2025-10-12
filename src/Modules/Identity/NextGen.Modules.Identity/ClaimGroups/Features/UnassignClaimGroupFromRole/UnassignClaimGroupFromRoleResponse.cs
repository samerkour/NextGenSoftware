namespace NextGen.Modules.Identity.ClaimGroups.Features.UnassignClaimGroupFromRole
{
    public class UnassignClaimGroupFromRoleResponse
    {
        public Guid ClaimGroupId { get; set; }
        public Guid RoleId { get; set; }
        public bool Success { get; set; }
    }
}
