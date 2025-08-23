using BuildingBlocks.Core.Messaging;

namespace NextGen.Modules.Customers.RestockSubscriptions.Features.CreatingRestockSubscription.Events.Integration;

public record RestockSubscriptionCreated(long CustomerId, string? Email) : IntegrationEvent;

