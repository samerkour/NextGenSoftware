using BuildingBlocks.Abstractions.CQRS.Command;

namespace NextGen.Modules.Identity.Roles.Features.DeleteRole
{
    public record DeleteRoleCommand(Guid Id, bool IsDeleted) : ICommand<bool>;
}
