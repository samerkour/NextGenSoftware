using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Abstractions.CQRS.Event.Internal;
using BuildingBlocks.Core.CQRS.Event.Internal;
using NextGen.Modules.Parties.RestockSubscriptions.Models.Write;
using NextGen.Modules.Parties.Shared.Data;

namespace NextGen.Modules.Parties.RestockSubscriptions.Features.DeletingRestockSubscription;

public record RestockSubscriptionDeleted(RestockSubscription RestockSubscription) : DomainEvent;

internal class RestockSubscriptionDeletedHandler : IDomainEventHandler<RestockSubscriptionDeleted>
{
    private readonly ICommandProcessor _commandProcessor;
    private readonly IMapper _mapper;
    private readonly PartiesDbContext _partiesDbContext;

    public RestockSubscriptionDeletedHandler(
        ICommandProcessor commandProcessor,
        IMapper mapper,
        PartiesDbContext partiesDbContext)
    {
        _commandProcessor = commandProcessor;
        _mapper = mapper;
        _partiesDbContext = partiesDbContext;
    }

    public async Task Handle(RestockSubscriptionDeleted notification, CancellationToken cancellationToken)
    {
        Guard.Against.Null(notification, nameof(notification));

        // var isDeleted = (bool)_partiesDbContext.Entry(notification.RestockSubscription)
        //     .Property("IsDeleted")
        //     .CurrentValue!;

        // https://github.com/kgrzybek/modular-monolith-with-ddd#38-internal-processing
        await _commandProcessor.SendAsync(
            new UpdateMongoRestockSubscriptionReadModel(notification.RestockSubscription, true),
            cancellationToken);
    }
}
