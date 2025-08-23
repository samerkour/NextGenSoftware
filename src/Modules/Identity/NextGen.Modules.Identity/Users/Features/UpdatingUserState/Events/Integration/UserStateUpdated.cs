using BuildingBlocks.Core.Messaging;
using NextGen.Modules.Identity.Shared.Models;

namespace NextGen.Modules.Identity.Users.Features.UpdatingUserState.Events.Integration;

public record UserStateUpdated(Guid UserId, UserState OldUserState, UserState NewUserState) : IntegrationEvent;
