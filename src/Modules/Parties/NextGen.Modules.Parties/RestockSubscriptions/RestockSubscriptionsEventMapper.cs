using BuildingBlocks.Abstractions.CQRS.Event;
using BuildingBlocks.Abstractions.CQRS.Event.Internal;
using BuildingBlocks.Abstractions.Messaging;
using NextGen.Modules.Parties.RestockSubscriptions.Features.CreatingRestockSubscription.Events.Domain;

namespace NextGen.Modules.Parties.RestockSubscriptions;

public class RestockSubscriptionsEventMapper : IIntegrationEventMapper
{
    public IReadOnlyList<IIntegrationEvent?> MapToIntegrationEvents(IReadOnlyList<IDomainEvent> domainEvents)
    {
        return domainEvents.Select(MapToIntegrationEvent).ToList();
    }

    public IIntegrationEvent? MapToIntegrationEvent(IDomainEvent domainEvent)
    {
        return domainEvent switch
        {
            RestockSubscriptionCreated e =>
                new Features.CreatingRestockSubscription.Events.Integration.RestockSubscriptionCreated(
                    e.RestockSubscription.Id.Value, e.RestockSubscription.Email),
            _ => null
        };
    }
}
