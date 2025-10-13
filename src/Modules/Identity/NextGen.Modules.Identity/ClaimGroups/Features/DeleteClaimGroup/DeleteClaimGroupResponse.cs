namespace NextGen.Modules.Identity.ClaimGroups.Features.DeleteClaimGroup;

public class DeleteClaimGroupResponse
{
    public Guid GroupId { get; set; }
    public bool IsDeleted { get; set; }
    public string Message { get; set; } = string.Empty;
}
