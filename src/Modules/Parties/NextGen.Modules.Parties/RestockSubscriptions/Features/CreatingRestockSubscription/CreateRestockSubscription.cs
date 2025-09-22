using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Core.Domain.ValueObjects;
using BuildingBlocks.Core.Exception;
using BuildingBlocks.Core.IdsGenerator;
using NextGen.Modules.Parties.Parties.Exceptions.Application;
using NextGen.Modules.Parties.Products.Exceptions;
using NextGen.Modules.Parties.RestockSubscriptions.Dtos;
using NextGen.Modules.Parties.RestockSubscriptions.Features.CreatingRestockSubscription.Exceptions;
using NextGen.Modules.Parties.RestockSubscriptions.Models.Write;
using NextGen.Modules.Parties.RestockSubscriptions.ValueObjects;
using NextGen.Modules.Parties.Shared.Clients.Inventories;
using NextGen.Modules.Parties.Shared.Data;
using NextGen.Modules.Parties.Shared.Extensions;
using FluentValidation;

namespace NextGen.Modules.Parties.RestockSubscriptions.Features.CreatingRestockSubscription;

public record CreateRestockSubscription(long PartyId, long ProductId, string Email)
    : ITxCreateCommand<CreateRestockSubscriptionResponse>
{
    public long Id { get; init; } = SnowFlakIdGenerator.NewId();
}

internal class CreateRestockSubscriptionValidator : AbstractValidator<CreateRestockSubscription>
{
    public CreateRestockSubscriptionValidator()
    {
        RuleFor(x => x.PartyId)
            .NotEmpty();

        RuleFor(x => x.ProductId)
            .NotEmpty();

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
    }
}

internal class CreateRestockSubscriptionHandler
    : ICommandHandler<CreateRestockSubscription, CreateRestockSubscriptionResponse>
{
    private readonly PartiesDbContext _partiesDbContext;
    private readonly IInventoryApiClient _inventoryApiClient;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateRestockSubscriptionHandler> _logger;

    public CreateRestockSubscriptionHandler(
        PartiesDbContext partiesDbContext,
        IInventoryApiClient inventoryApiClient,
        IMapper mapper,
        ILogger<CreateRestockSubscriptionHandler> logger)
    {
        _partiesDbContext = partiesDbContext;
        _inventoryApiClient = inventoryApiClient;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CreateRestockSubscriptionResponse> Handle(
        CreateRestockSubscription request,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        var existsParty = await _partiesDbContext.ExistsPartyByIdAsync(request.PartyId);
        Guard.Against.NotExists(existsParty, new PartyNotFoundException(request.PartyId));

        var product = (await _inventoryApiClient.GetProductByIdAsync(request.ProductId, cancellationToken))?.Product;
        Guard.Against.NotFound(product, new ProductNotFoundException(request.ProductId));

        if (product!.AvailableStock > 0)
            throw new ProductHaveStockException(product.Id, product.AvailableStock, product.Name);

        var alreadySubscribed = _partiesDbContext.RestockSubscriptions
            .Any(x => x.Email == request.Email && x.ProductInformation.Id == request.ProductId && x.Processed == false);

        if (alreadySubscribed)
            throw new ProductAlreadySubscribedException(product.Id, product.Name);

        var restockSubscription =
            RestockSubscription.Create(
                request.Id,
                request.PartyId,
                ProductInformation.Create(product!.Id, product.Name),
                Email.Create(request.Email));

        await _partiesDbContext.AddAsync(restockSubscription, cancellationToken);

        await _partiesDbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("RestockSubscription with id '{@Id}' saved successfully", restockSubscription.Id);

        var restockSubscriptionDto = _mapper.Map<RestockSubscriptionDto>(restockSubscription);

        return new CreateRestockSubscriptionResponse(restockSubscriptionDto);
    }
}
