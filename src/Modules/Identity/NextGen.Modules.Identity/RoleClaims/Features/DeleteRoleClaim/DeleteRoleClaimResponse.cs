namespace NextGen.Modules.Identity.RoleClaims.Features.DeleteRoleClaim
{
    public class DeleteRoleClaimResponse
    {
        public Guid RoleId { get; set; }
        public Guid ClaimId { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
