namespace NextGen.Modules.Identity.Roles.Features.GetRoleClaimGroups
{
    public class GetRoleClaimGroupsResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }
}
