using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Query;
using BuildingBlocks.Core.CQRS.Query;
using BuildingBlocks.Core.Types;
using BuildingBlocks.Persistence.Mongo;
using NextGen.Modules.Parties.Parties.Dtos;
using NextGen.Modules.Parties.Parties.Models.Reads;
using NextGen.Modules.Parties.Shared.Data;
using FluentValidation;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace NextGen.Modules.Parties.Parties.Features.GettingParties;

public record GetParties : ListQuery<GetPartiesResponse>;

public class GetPartiesValidator : AbstractValidator<GetParties>
{
    public GetPartiesValidator()
    {
        CascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1).WithMessage("Page should at least greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("PageSize should at least greater than or equal to 1.");
    }
}

public class GetPartiesHandler : IQueryHandler<GetParties, GetPartiesResponse>
{
    private readonly PartiesReadDbContext _partiesReadDbContext;
    private readonly IMapper _mapper;

    public GetPartiesHandler(PartiesReadDbContext partiesReadDbContext, IMapper mapper)
    {
        _partiesReadDbContext = partiesReadDbContext;
        _mapper = mapper;
    }

    public async Task<GetPartiesResponse> Handle(GetParties request, CancellationToken cancellationToken)
    {
        var party = await _partiesReadDbContext.Parties.AsQueryable()
            .OrderByDescending(x => x.City)
            .ApplyFilter(request.Filters)
            .ApplyPagingAsync<PartyReadModel, PartyReadDto>(
                _mapper.ConfigurationProvider,
                request.Page,
                request.PageSize,
                cancellationToken: cancellationToken);

        return new GetPartiesResponse(party);
    }
}
