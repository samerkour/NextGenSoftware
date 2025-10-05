using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Query;
using BuildingBlocks.Core.Exception;
using NextGen.Modules.Parties.Parties.Dtos;
using NextGen.Modules.Parties.Parties.Exceptions.Application;
using NextGen.Modules.Parties.Shared.Data;
using FluentValidation;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace NextGen.Modules.Parties.Parties.Features.GettingPartyById;

public record GetPartyById(long Id) : IQuery<GetPartyByIdResponse>;

internal class GetPartyByIdValidator : AbstractValidator<GetPartyById>
{
    public GetPartyByIdValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}

internal class GetRestockSubscriptionByIdHandler
    : IQueryHandler<GetPartyById, GetPartyByIdResponse>
{
    private readonly PartiesReadDbContext _partiesReadDbContext;
    private readonly IMapper _mapper;

    public GetRestockSubscriptionByIdHandler(PartiesReadDbContext partiesReadDbContext, IMapper mapper)
    {
        _partiesReadDbContext = partiesReadDbContext;
        _mapper = mapper;
    }

    public async Task<GetPartyByIdResponse> Handle(
        GetPartyById query,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(query, nameof(query));

        var party = await _partiesReadDbContext.Parties.AsQueryable()
            .SingleOrDefaultAsync(x => x.PartyId == query.Id, cancellationToken: cancellationToken);

        Guard.Against.NotFound(party, new PartyNotFoundException(query.Id));

        var partyDto = _mapper.Map<PartyReadDto>(party);

        return new GetPartyByIdResponse(partyDto);
    }
}
