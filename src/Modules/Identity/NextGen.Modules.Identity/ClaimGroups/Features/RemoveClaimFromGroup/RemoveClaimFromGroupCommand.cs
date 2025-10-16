using BuildingBlocks.Abstractions.CQRS.Command;

namespace NextGen.Modules.Identity.ClaimGroups.Features.RemoveClaimFromGroup
{
    public record RemoveClaimFromGroupCommand(Guid GroupId, Guid ClaimId, bool IsDeleted)
        : ICommand<bool>;
}
