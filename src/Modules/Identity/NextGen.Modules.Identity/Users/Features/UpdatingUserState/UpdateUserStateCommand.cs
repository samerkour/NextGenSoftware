using BuildingBlocks.Abstractions.CQRS.Command;
using NextGen.Modules.Identity.Shared.Models;

namespace NextGen.Modules.Identity.Users.Features.UpdatingUserState;

public record UpdateUserStateCommand(Guid UserId, UserState State) : ITxUpdateCommand;
