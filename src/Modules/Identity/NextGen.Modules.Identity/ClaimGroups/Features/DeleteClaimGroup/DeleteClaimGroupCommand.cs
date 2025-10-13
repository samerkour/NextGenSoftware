using BuildingBlocks.Abstractions.CQRS.Command;

namespace NextGen.Modules.Identity.ClaimGroups.Features.DeleteClaimGroup;

public record DeleteClaimGroupCommand(Guid GroupId, bool IsDeleted) : ICommand<DeleteClaimGroupResponse>;
