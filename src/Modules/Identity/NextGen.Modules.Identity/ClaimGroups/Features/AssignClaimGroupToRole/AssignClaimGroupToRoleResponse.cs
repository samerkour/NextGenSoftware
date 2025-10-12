using BuildingBlocks.Abstractions.CQRS.Command;
public class AssignClaimGroupToRoleResponse
{
    public Guid ClaimGroupId { get; set; }
    public Guid RoleId { get; set; }
    public string Message { get; set; } = string.Empty;
}
