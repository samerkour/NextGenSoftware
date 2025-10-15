using BuildingBlocks.Abstractions.CQRS.Command;

namespace NextGen.Modules.Identity.Roles.Features.UpdateRole;

public record UpdateRoleCommand(
    Guid Id,
    string Name,
    string? Description
) : ICommand<UpdateRoleResponse>;
