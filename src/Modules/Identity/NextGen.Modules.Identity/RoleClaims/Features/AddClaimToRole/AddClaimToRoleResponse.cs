public class AddClaimToRoleResponse
{
    public Guid RoleId { get; set; }
    public Guid ClaimId { get; set; }
    public string Message { get; set; } = string.Empty;
}
