using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Event.Internal;
using BuildingBlocks.Core.CQRS.Event.Internal;
using NextGen.Modules.Inventories.Products.ValueObjects;
using NextGen.Modules.Inventories.Shared.Contracts;

namespace NextGen.Modules.Inventories.Products.Features.DebitingProductStock.Events.Domain;

public record ProductRestockThresholdReachedEvent(ProductId ProductId, Stock Stock, int Quantity) : DomainEvent;

internal class ProductRestockThresholdReachedEventHandler : IDomainEventHandler<ProductRestockThresholdReachedEvent>
{
    private readonly IInventoryDbContext _inventoryDbContext;

    public ProductRestockThresholdReachedEventHandler(IInventoryDbContext inventoryDbContext)
    {
        _inventoryDbContext = inventoryDbContext;
    }

    public Task Handle(ProductRestockThresholdReachedEvent notification, CancellationToken cancellationToken)
    {
        Guard.Against.Null(notification, nameof(notification));

        // For example send an email to get more products
        return Task.CompletedTask;
    }
}
