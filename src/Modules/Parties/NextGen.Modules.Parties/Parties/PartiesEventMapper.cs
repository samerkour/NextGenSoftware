using BuildingBlocks.Abstractions.CQRS.Event;
using BuildingBlocks.Abstractions.CQRS.Event.Internal;
using BuildingBlocks.Abstractions.Messaging;
using NextGen.Modules.Parties.Parties.Features.CreatingParty.Events.Domain;

namespace NextGen.Modules.Parties.Parties;

public class PartiesEventMapper : IIntegrationEventMapper
{
    public IReadOnlyList<IIntegrationEvent?> MapToIntegrationEvents(IReadOnlyList<IDomainEvent> domainEvents)
    {
        return domainEvents.Select(MapToIntegrationEvent).ToList();
    }

    public IIntegrationEvent? MapToIntegrationEvent(IDomainEvent domainEvent)
    {
        return domainEvent switch
        {
            PartyCreated e => new Features.CreatingParty.Events.Integration.PartyCreated(e.Party.Id),
            _ => null
        };
    }
}
