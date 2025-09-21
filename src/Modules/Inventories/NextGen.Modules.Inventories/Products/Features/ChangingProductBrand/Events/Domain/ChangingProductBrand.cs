using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Event.Internal;
using BuildingBlocks.Core.CQRS.Event.Internal;
using NextGen.Modules.Inventorys.Brands;
using NextGen.Modules.Inventorys.Shared.Contracts;
using NextGen.Modules.Inventorys.Shared.Extensions;

namespace NextGen.Modules.Inventorys.Products.Features.ChangingProductBrand.Events.Domain;

internal record ChangingProductBrand(BrandId BrandId) : DomainEvent;

internal class ChangingProductBrandValidationHandler :
    IDomainEventHandler<ChangingProductBrand>
{
    private readonly IInventoryDbContext _inventoryDbContext;

    public ChangingProductBrandValidationHandler(IInventoryDbContext inventoryDbContext)
    {
        _inventoryDbContext = inventoryDbContext;
    }

    public async Task Handle(ChangingProductBrand notification, CancellationToken cancellationToken)
    {
        // Handling some validations
        Guard.Against.Null(notification, nameof(notification));
        Guard.Against.NegativeOrZero(notification.BrandId, nameof(notification.BrandId));
        Guard.Against.ExistsBrand(
            await _inventoryDbContext.BrandExistsAsync(notification.BrandId, cancellationToken: cancellationToken),
            notification.BrandId);
    }
}
