using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Query;
using BuildingBlocks.Core.Exception;
using NextGen.Modules.Parties.RestockSubscriptions.Dtos;
using NextGen.Modules.Parties.RestockSubscriptions.Exceptions.Application;
using NextGen.Modules.Parties.Shared.Data;
using FluentValidation;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace NextGen.Modules.Parties.RestockSubscriptions.Features.GettingRestockSubscriptionById;

public record GetRestockSubscriptionById(long Id) : IQuery<GetRestockSubscriptionByIdResponse>;

internal class GetRestockSubscriptionByIdValidator : AbstractValidator<GetRestockSubscriptionById>
{
    public GetRestockSubscriptionByIdValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}

internal class GetRestockSubscriptionByIdHandler
    : IQueryHandler<GetRestockSubscriptionById, GetRestockSubscriptionByIdResponse>
{
    private readonly PartiesReadDbContext _partiesReadDbContext;
    private readonly IMapper _mapper;

    public GetRestockSubscriptionByIdHandler(PartiesReadDbContext partiesReadDbContext, IMapper mapper)
    {
        _partiesReadDbContext = partiesReadDbContext;
        _mapper = mapper;
    }

    public async Task<GetRestockSubscriptionByIdResponse> Handle(
        GetRestockSubscriptionById query,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(query, nameof(query));

        var restockSubscription =
            await _partiesReadDbContext.RestockSubscriptions.AsQueryable()
                .Where(x => x.IsDeleted == false)
                .SingleOrDefaultAsync(x => x.RestockSubscriptionId == query.Id, cancellationToken: cancellationToken);

        Guard.Against.NotFound(restockSubscription, new RestockSubscriptionNotFoundException(query.Id));

        var subscriptionDto = _mapper.Map<RestockSubscriptionDto>(restockSubscription);

        return new GetRestockSubscriptionByIdResponse(subscriptionDto);
    }
}
