namespace NextGen.Modules.Identity.Roles.Features.GetRoleById
{
    public class GetRoleByIdResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }
}
