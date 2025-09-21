using Ardalis.GuardClauses;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BuildingBlocks.Abstractions.CQRS.Query;
using NextGen.Modules.Parties.RestockSubscriptions.Dtos;
using NextGen.Modules.Parties.Shared.Data;
using FluentValidation;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace NextGen.Modules.Parties.RestockSubscriptions.Features.GettingRestockSubscriptionsByEmails;

public record GetRestockSubscriptionsByEmails(IList<string> Emails) : IStreamQuery<RestockSubscriptionDto>;

internal class GetRestockSubscriptionsByEmailsValidator : AbstractValidator<GetRestockSubscriptionsByEmails>
{
    public GetRestockSubscriptionsByEmailsValidator()
    {
        CascadeMode = CascadeMode.Stop;

        RuleFor(request => request.Emails)
            .NotNull()
            .NotEmpty();
    }
}

internal class GetRestockSubscriptionsByEmailsHandler
    : IStreamQueryHandler<GetRestockSubscriptionsByEmails, RestockSubscriptionDto>
{
    private readonly PartiesReadDbContext _partiesReadDbContext;
    private readonly IMapper _mapper;

    public GetRestockSubscriptionsByEmailsHandler(PartiesReadDbContext partiesReadDbContext, IMapper mapper)
    {
        _partiesReadDbContext = partiesReadDbContext;
        _mapper = mapper;
    }

    public IAsyncEnumerable<RestockSubscriptionDto> Handle(
        GetRestockSubscriptionsByEmails query,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(query, nameof(query));

        var result = _partiesReadDbContext.RestockSubscriptions.AsQueryable()
            .Where(x => !x.IsDeleted)
            .Where(x => query.Emails.Contains(x.Email!))
            .ProjectTo<RestockSubscriptionDto>(_mapper.ConfigurationProvider)
            .ToAsyncEnumerable();

        return result;
    }
}
