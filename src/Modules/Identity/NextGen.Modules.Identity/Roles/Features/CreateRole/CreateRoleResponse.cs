namespace NextGen.Modules.Identity.Roles.Features.CreateRole
{
    public class CreateRoleResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
