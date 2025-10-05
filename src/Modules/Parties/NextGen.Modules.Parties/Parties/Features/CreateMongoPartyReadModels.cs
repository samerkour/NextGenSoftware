using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Core.CQRS.Command;
using NextGen.Modules.Parties.Parties.Models.Reads;
using NextGen.Modules.Parties.Shared.Data;

namespace NextGen.Modules.Parties.Parties.Features;

public record CreateMongoPartyReadModels : InternalCommand
{
    public long PartyId { get; init; }
    public new Guid Id { get; init; }
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
    public DateTime Created { get; init; }
}

internal class CreateMongoPartyReadModelsHandler : ICommandHandler<CreateMongoPartyReadModels>
{
    private readonly PartiesReadDbContext _partiesReadDbContext;
    private readonly IMapper _mapper;

    public CreateMongoPartyReadModelsHandler(PartiesReadDbContext partiesReadDbContext, IMapper mapper)
    {
        _partiesReadDbContext = partiesReadDbContext;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(CreateMongoPartyReadModels command, CancellationToken cancellationToken)
    {
        Guard.Against.Null(command, nameof(command));

        var readModel = _mapper.Map<PartyReadModel>(command);

        await _partiesReadDbContext.Parties.InsertOneAsync(readModel, cancellationToken: cancellationToken);

        return Unit.Value;
    }
}
