using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Event.Internal;
using BuildingBlocks.Core.CQRS.Event.Internal;
using NextGen.Modules.Inventories.Shared.Contracts;
using NextGen.Modules.Inventories.Shared.Extensions;
using NextGen.Modules.Inventories.Suppliers;

namespace NextGen.Modules.Inventories.Products.Features.ChangingProductSupplier.Events;

public record ChangingProductSupplier(SupplierId SupplierId) : DomainEvent;

internal class ChangingSupplierValidationHandler :
    IDomainEventHandler<ChangingProductSupplier>
{
    private readonly IInventoryDbContext _inventoryDbContext;

    public ChangingSupplierValidationHandler(IInventoryDbContext inventoryDbContext)
    {
        _inventoryDbContext = inventoryDbContext;
    }

    public async Task Handle(ChangingProductSupplier notification, CancellationToken cancellationToken)
    {
        Guard.Against.Null(notification, nameof(notification));
        Guard.Against.NegativeOrZero(notification.SupplierId, nameof(notification.SupplierId));
        Guard.Against.ExistsSupplier(
            await _inventoryDbContext.SupplierExistsAsync(notification.SupplierId, cancellationToken: cancellationToken),
            notification.SupplierId);
    }
}
