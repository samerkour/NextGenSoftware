using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Core.CQRS.Command;
using NextGen.Modules.Parties.RestockSubscriptions.Models.Read;
using NextGen.Modules.Parties.RestockSubscriptions.Models.Write;
using NextGen.Modules.Parties.Shared.Data;
using MongoDB.Driver;

namespace NextGen.Modules.Parties.RestockSubscriptions.Features;

public record UpdateMongoRestockSubscriptionReadModel
    (RestockSubscription RestockSubscription, bool IsDeleted) : InternalCommand;

internal class UpdateMongoRestockSubscriptionReadModelHandler : ICommandHandler<UpdateMongoRestockSubscriptionReadModel>
{
    private readonly PartiesReadDbContext _partiesReadDbContext;
    private readonly IMapper _mapper;

    public UpdateMongoRestockSubscriptionReadModelHandler(PartiesReadDbContext partiesReadDbContext, IMapper mapper)
    {
        _partiesReadDbContext = partiesReadDbContext;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(
        UpdateMongoRestockSubscriptionReadModel command, CancellationToken cancellationToken)
    {
        Guard.Against.Null(command, nameof(command));

        var filterDefinition =
            Builders<RestockSubscriptionReadModel>.Filter
                .Eq(x => x.RestockSubscriptionId, command.RestockSubscription.Id.Value);

        var updateDefinition =
            Builders<RestockSubscriptionReadModel>.Update
                .Set(x => x.Email, command.RestockSubscription.Email.Value)
                .Set(x => x.ProductName, command.RestockSubscription.ProductInformation.Name)
                .Set(x => x.ProductId, command.RestockSubscription.ProductInformation.Id.Value)
                .Set(x => x.Processed, command.RestockSubscription.Processed)
                .Set(x => x.ProcessedTime, command.RestockSubscription.ProcessedTime)
                .Set(x => x.PartyId, command.RestockSubscription.PartyId.Value)
                .Set(x => x.IsDeleted, command.IsDeleted)
                .Set(x => x.RestockSubscriptionId, command.RestockSubscription.Id.Value);

        await _partiesReadDbContext.RestockSubscriptions.UpdateOneAsync(
            filterDefinition,
            updateDefinition,
            new UpdateOptions(),
            cancellationToken);

        // await _partiesReadDbContext.RestockSubscriptions.ReplaceOneAsync(
        //     x => x.RestockSubscriptionId == command.RestockSubscription.Id.Value,
        //     updatedEntity,
        //     new ReplaceOptions(),
        //     cancellationToken);

        return Unit.Value;
    }
}
