using BuildingBlocks.Core.Messaging;

namespace NextGen.Modules.Identity.Users.Features.RegisteringUser.Events.Integration;

public record UserRegistered(
    Guid IdentityId,
    string Email,
    string UserName,
    string FirstName,
    string LastName,
    IList<string>? Roles) : IntegrationEvent;
