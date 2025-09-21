using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Core.CQRS.Command;
using NextGen.Modules.Parties.RestockSubscriptions.Models.Read;
using NextGen.Modules.Parties.Shared.Data;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace NextGen.Modules.Parties.RestockSubscriptions.Features;

public record UpdateMongoRestockSubscriptionsReadModelByTime
    (DateTime? From, DateTime? To, bool IsDeleted = false) : InternalCommand;

internal class UpdateMongoRestockSubscriptionsReadModelByTimeHandler
    : ICommandHandler<UpdateMongoRestockSubscriptionsReadModelByTime>
{
    private readonly IMapper _mapper;
    private readonly PartiesReadDbContext _partiesReadDbContext;

    public UpdateMongoRestockSubscriptionsReadModelByTimeHandler(
        IMapper mapper,
        PartiesReadDbContext partiesReadDbContext)
    {
        _mapper = mapper;
        _partiesReadDbContext = partiesReadDbContext;
    }

    public async Task<Unit> Handle(
        UpdateMongoRestockSubscriptionsReadModelByTime command,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(command, nameof(command));

        var itemsToUpdate = await _partiesReadDbContext.RestockSubscriptions.AsQueryable()
            .Where(x => (command.From == null && command.To == null) ||
                        (command.From == null && x.Created <= command.To) ||
                        (command.To == null && x.Created >= command.From) ||
                        (x.Created >= command.From && x.Created <= command.To))
            .ToListAsync(cancellationToken);

        if (itemsToUpdate.Any() == false)
            return Unit.Value;

        var listWrites = new List<WriteModel<RestockSubscriptionReadModel>>();

        // https://mongodb.github.io/mongo-csharp-driver/2.7/reference/driver/crud/writing/
        // https://mongodb.github.io/mongo-csharp-driver/2.7/reference/driver/crud/reading/
        // https://mongodb.github.io/mongo-csharp-driver/2.7/reference/driver/crud/linq/
        // https://mongodb.github.io/mongo-csharp-driver/2.7/reference/driver/crud/sessions_and_transactions/
        // https://fgambarino.com/c-sharp-mongo-bulk-write/
        foreach (var restockSubscriptionReadModel in itemsToUpdate)
        {
            var filterDefinition =
                Builders<RestockSubscriptionReadModel>.Filter
                    .Eq(x => x.RestockSubscriptionId, restockSubscriptionReadModel.RestockSubscriptionId);

            var updateDefinition =
                Builders<RestockSubscriptionReadModel>.Update
                    .Set(x => x.Email, restockSubscriptionReadModel.Email)
                    .Set(x => x.ProductName, restockSubscriptionReadModel.ProductName)
                    .Set(x => x.ProductId, restockSubscriptionReadModel.ProductId)
                    .Set(x => x.Processed, restockSubscriptionReadModel.Processed)
                    .Set(x => x.ProcessedTime, restockSubscriptionReadModel.ProcessedTime)
                    .Set(x => x.PartyId, restockSubscriptionReadModel.PartyId)
                    .Set(x => x.IsDeleted, command.IsDeleted)
                    .Set(x => x.RestockSubscriptionId, restockSubscriptionReadModel.RestockSubscriptionId);

            listWrites.Add(new UpdateOneModel<RestockSubscriptionReadModel>(filterDefinition, updateDefinition));
        }

        await _partiesReadDbContext.RestockSubscriptions.BulkWriteAsync(
            listWrites,
            cancellationToken: cancellationToken);

        listWrites.Clear();

        return Unit.Value;
    }
}
