using BuildingBlocks.Core.CQRS.Event.Internal;
using NextGen.Modules.Inventories.Categories;
using NextGen.Modules.Inventories.Products.ValueObjects;

namespace NextGen.Modules.Inventories.Products.Features.ChangingProductCategory.Events;

public record ProductCategoryChanged(CategoryId CategoryId, ProductId ProductId) :
    DomainEvent;
