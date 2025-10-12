using BuildingBlocks.Abstractions.CQRS.Command;

namespace NextGen.Modules.Identity.ClaimGroups.Features.UpdateClaimGroup
{
    public record UpdateClaimGroupCommand(
        Guid Id,
        string Name,
        string? Description
    ) : ICommand<UpdateClaimGroupResponse>;
}
