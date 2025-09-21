using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Event.Internal;
using BuildingBlocks.Core.CQRS.Event.Internal;
using NextGen.Modules.Inventorys.Categories;
using NextGen.Modules.Inventorys.Shared.Contracts;
using NextGen.Modules.Inventorys.Shared.Extensions;

namespace NextGen.Modules.Inventorys.Products.Features.ChangingProductCategory.Events;

public record ChangingProductCategory(CategoryId CategoryId) : DomainEvent;

internal class ChangingProductCategoryValidationHandler :
    IDomainEventHandler<ChangingProductCategory>
{
    private readonly IInventoryDbContext _inventoryDbContext;

    public ChangingProductCategoryValidationHandler(IInventoryDbContext inventoryDbContext)
    {
        _inventoryDbContext = inventoryDbContext;
    }

    public async Task Handle(ChangingProductCategory notification, CancellationToken cancellationToken)
    {
        Guard.Against.Null(notification, nameof(notification));
        Guard.Against.NegativeOrZero(notification.CategoryId, nameof(notification.CategoryId));
        Guard.Against.ExistsCategory(
            await _inventoryDbContext.CategoryExistsAsync(notification.CategoryId, cancellationToken: cancellationToken),
            notification.CategoryId);
    }
}
