namespace NextGen.Modules.Identity.Roles.Features.UpdateRole;

public class UpdateRoleRequest
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
}
