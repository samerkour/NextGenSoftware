using BuildingBlocks.Abstractions.Messaging;
using BuildingBlocks.Abstractions.Messaging.Context;
using BuildingBlocks.Abstractions.Persistence;
using BuildingBlocks.Core.Messaging;

namespace NextGen.Modules.Notifications.Parties.Features.CreatingParty.Events.External;

public record PartyCreated(long PartyId) : IntegrationEvent, ITxRequest;

public class PartyCreatedConsumer : IMessageHandler<PartyCreated>
{
    public Task HandleAsync(
        IConsumeContext<PartyCreated> messageContext,
        CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
