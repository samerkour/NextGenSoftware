using BuildingBlocks.Core.CQRS.Event.Internal;
using NextGen.Modules.Inventorys.Categories;
using NextGen.Modules.Inventorys.Products.ValueObjects;

namespace NextGen.Modules.Inventorys.Products.Features.ChangingProductCategory.Events;

public record ProductCategoryChanged(CategoryId CategoryId, ProductId ProductId) :
    DomainEvent;
