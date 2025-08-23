using BuildingBlocks.Core.CQRS.Event.Internal;
using NextGen.Modules.Catalogs.Categories;
using NextGen.Modules.Catalogs.Products.ValueObjects;

namespace NextGen.Modules.Catalogs.Products.Features.ChangingProductCategory.Events;

public record ProductCategoryChangedNotification(CategoryId CategoryId, ProductId ProductId) : DomainEvent;
