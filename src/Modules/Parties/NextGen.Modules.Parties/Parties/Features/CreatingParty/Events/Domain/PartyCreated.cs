using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Abstractions.CQRS.Event.Internal;
using BuildingBlocks.Core.CQRS.Event.Internal;
using NextGen.Modules.Parties.Parties.Models;

namespace NextGen.Modules.Parties.Parties.Features.CreatingParty.Events.Domain;

public record PartyCreated(Party Party) : DomainEvent;

internal class PartyCreatedHandler : IDomainEventHandler<PartyCreated>
{
    private readonly ICommandProcessor _commandProcessor;
    private readonly IMapper _mapper;

    public PartyCreatedHandler(ICommandProcessor commandProcessor, IMapper mapper)
    {
        _commandProcessor = commandProcessor;
        _mapper = mapper;
    }

    public Task Handle(PartyCreated notification, CancellationToken cancellationToken)
    {
        Guard.Against.Null(notification, nameof(notification));

        var mongoReadCommand = _mapper.Map<CreateMongoPartyReadModels>(notification.Party);

        // https://github.com/kgrzybek/modular-monolith-with-ddd#38-internal-processing
        // Schedule multiple read sides to execute here
        return _commandProcessor.ScheduleAsync(new IInternalCommand[] { mongoReadCommand }, cancellationToken);
    }
}
