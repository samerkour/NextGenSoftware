using BuildingBlocks.Core.CQRS.Event.Internal;
using NextGen.Modules.Inventories.Brands;
using NextGen.Modules.Inventories.Products.ValueObjects;

namespace NextGen.Modules.Inventories.Products.Features.ChangingProductBrand.Events.Domain;

internal record ProductBrandChanged(BrandId BrandId, ProductId ProductId) : DomainEvent;
