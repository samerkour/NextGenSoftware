using BuildingBlocks.Abstractions.CQRS.Command;

namespace NextGen.Modules.Identity.Roles.Features.CreateRole
{
    public record CreateRoleCommand(string Name, string? Description)
        : ICommand<CreateRoleResponse>;
}
