using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Core.CQRS.Command;
using NextGen.Modules.Parties.RestockSubscriptions.Models.Read;
using NextGen.Modules.Parties.Shared.Data;

namespace NextGen.Modules.Parties.RestockSubscriptions.Features;

public record CreateMongoRestockSubscriptionReadModels(
    long RestockSubscriptionId,
    long PartyId,
    string PartyName,
    long ProductId,
    string ProductName,
    string Email,
    DateTime Created,
    bool Processed,
    DateTime? ProcessedTime = null) : InternalCommand
{
    public bool IsDeleted { get; init; }
}

internal class CreateRestockSubscriptionReadModelHandler : ICommandHandler<CreateMongoRestockSubscriptionReadModels>
{
    private readonly PartiesReadDbContext _mongoDbContext;
    private readonly IMapper _mapper;

    public CreateRestockSubscriptionReadModelHandler(PartiesReadDbContext mongoDbContext, IMapper mapper)
    {
        _mongoDbContext = mongoDbContext;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(
        CreateMongoRestockSubscriptionReadModels command,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(command, nameof(command));

        var readModel = _mapper.Map<RestockSubscriptionReadModel>(command);

        await _mongoDbContext.RestockSubscriptions.InsertOneAsync(readModel, cancellationToken: cancellationToken);

        return Unit.Value;
    }
}
