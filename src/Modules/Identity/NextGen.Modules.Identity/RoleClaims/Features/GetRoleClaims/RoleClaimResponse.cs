namespace NextGen.Modules.Identity.Roles.Features.GetRoleClaims
{
    public class RoleClaimResponse
    {
        public Guid ClaimId { get; set; }
        public string Type { get; set; } = default!;
        public string Value { get; set; } = default!;
        public Guid RoleId { get; set; }
    }
}
