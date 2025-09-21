using BuildingBlocks.Core.Messaging;

namespace NextGen.Modules.Parties.RestockSubscriptions.Features.CreatingRestockSubscription.Events.Integration;

public record RestockSubscriptionCreated(long PartyId, string? Email) : IntegrationEvent;

