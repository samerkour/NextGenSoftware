namespace NextGen.Modules.Identity.Roles.Dtos;

public record RoleDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = default!;
    public string? Description { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedOn { get; init; }
    public DateTime? DeletedOn { get; init; }
}
