using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Core.CQRS.Command;
using NextGen.Modules.Parties.Parties.Models.Reads;
using NextGen.Modules.Parties.Shared.Data;
using MongoDB.Driver;

namespace NextGen.Modules.Parties.Parties.Features;

public record UpdateMongoPartyReadsModel : InternalCommand
{
    public new Guid Id { get; init; }
    public long PartyId { get; init; }
    public Guid IdentityId { get; init; }
    public string Email { get; init; } = null!;
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string FullName { get; init; } = null!;
    public string? Country { get; init; }
    public string? City { get; init; }
    public string? DetailAddress { get; init; }
    public string? Nationality { get; init; }
    public DateTime? BirthDate { get; init; }
    public string? PhoneNumber { get; init; }
}

internal class UpdateMongoPartyReadsModelHandler : ICommandHandler<UpdateMongoPartyReadsModel>
{
    private readonly PartiesReadDbContext _partiesReadDbContext;
    private readonly IMapper _mapper;

    public UpdateMongoPartyReadsModelHandler(PartiesReadDbContext partiesReadDbContext, IMapper mapper)
    {
        _partiesReadDbContext = partiesReadDbContext;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateMongoPartyReadsModel command, CancellationToken cancellationToken)
    {
        Guard.Against.Null(command, nameof(command));

        var filterDefinition =
            Builders<PartyReadModel>.Filter
                .Eq(x => x.PartyId, command.PartyId);

        var updateDefinition =
            Builders<PartyReadModel>.Update
                .Set(x => x.Email, command.Email)
                .Set(x => x.Country, command.Country)
                .Set(x => x.City, command.City)
                .Set(x => x.DetailAddress, command.DetailAddress)
                .Set(x => x.IdentityId, command.IdentityId)
                .Set(x => x.PartyId, command.PartyId)
                .Set(x => x.Nationality, command.Nationality)
                .Set(x => x.FirstName, command.FirstName)
                .Set(x => x.LastName, command.LastName)
                .Set(x => x.FullName, command.FullName)
                .Set(x => x.PhoneNumber, command.PhoneNumber)
                .Set(x => x.BirthDate, command.BirthDate);

        await _partiesReadDbContext.Parties.UpdateOneAsync(
            filterDefinition,
            updateDefinition,
            new UpdateOptions(),
            cancellationToken);

        return Unit.Value;
    }
}
