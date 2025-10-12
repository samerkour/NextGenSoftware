using BuildingBlocks.Abstractions.CQRS.Command;

public record AssignClaimGroupToRoleCommand(Guid ClaimGroupId, Guid RoleId)
    : ICommand<AssignClaimGroupToRoleResponse>;


