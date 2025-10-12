using BuildingBlocks.Abstractions.CQRS.Command;

namespace NextGen.Modules.Identity.ClaimGroups.Features.CreateClaimGroup
{
    public record CreateClaimGroupCommand(string Name, string? Description)
        : ICommand<CreateClaimGroupResponse>;
}
