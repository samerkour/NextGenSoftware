// DeleteClaimCommand.cs
using BuildingBlocks.Abstractions.CQRS.Command;

namespace NextGen.Modules.Identity.Claims.Features.DeleteClaim;

public record DeleteClaimCommand(Guid Id, bool IsDeleted) : ICommand<bool>;
