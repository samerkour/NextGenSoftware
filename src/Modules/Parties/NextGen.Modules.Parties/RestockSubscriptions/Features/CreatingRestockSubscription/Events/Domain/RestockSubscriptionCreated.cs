using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Abstractions.CQRS.Event.Internal;
using BuildingBlocks.Core.CQRS.Event.Internal;
using BuildingBlocks.Core.Exception;
using NextGen.Modules.Parties.Parties.Exceptions.Application;
using NextGen.Modules.Parties.RestockSubscriptions.Models.Write;
using NextGen.Modules.Parties.Shared.Data;
using NextGen.Modules.Parties.Shared.Extensions;

namespace NextGen.Modules.Parties.RestockSubscriptions.Features.CreatingRestockSubscription.Events.Domain;

public record RestockSubscriptionCreated(RestockSubscription RestockSubscription) : DomainEvent
{
    public CreateMongoRestockSubscriptionReadModels ToCreateMongoRestockSubscriptionReadModels(
        long partyId,
        string partyName)
    {
        return new CreateMongoRestockSubscriptionReadModels(
            RestockSubscription.Id,
            partyId,
            partyName,
            RestockSubscription.ProductInformation.Id,
            RestockSubscription.ProductInformation.Name,
            RestockSubscription.Email.Value,
            RestockSubscription.Created,
            RestockSubscription.Processed,
            RestockSubscription.ProcessedTime);
    }
}

internal class RestockSubscriptionCreatedHandler : IDomainEventHandler<RestockSubscriptionCreated>
{
    private readonly ICommandProcessor _commandProcessor;
    private readonly PartiesDbContext _partiesDbContext;

    public RestockSubscriptionCreatedHandler(ICommandProcessor commandProcessor, PartiesDbContext partiesDbContext)
    {
        _commandProcessor = commandProcessor;
        _partiesDbContext = partiesDbContext;
    }

    public async Task Handle(RestockSubscriptionCreated notification, CancellationToken cancellationToken)
    {
        Guard.Against.Null(notification, nameof(notification));

        var party = await _partiesDbContext.FindPartyByIdAsync(notification.RestockSubscription.PartyId);

        Guard.Against.NotFound(
            party,
            new PartyNotFoundException(notification.RestockSubscription.PartyId));

        var mongoReadCommand =
            notification.ToCreateMongoRestockSubscriptionReadModels(party!.Id, party.Name.FullName);

        // https://github.com/kgrzybek/modular-monolith-with-ddd#38-internal-processing
        // Schedule multiple read sides to execute here
        await _commandProcessor.ScheduleAsync(new IInternalCommand[] { mongoReadCommand }, cancellationToken);
    }
}
