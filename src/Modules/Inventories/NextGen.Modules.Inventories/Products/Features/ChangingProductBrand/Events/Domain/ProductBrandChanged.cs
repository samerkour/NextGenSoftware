using BuildingBlocks.Core.CQRS.Event.Internal;
using NextGen.Modules.Inventorys.Brands;
using NextGen.Modules.Inventorys.Products.ValueObjects;

namespace NextGen.Modules.Inventorys.Products.Features.ChangingProductBrand.Events.Domain;

internal record ProductBrandChanged(BrandId BrandId, ProductId ProductId) : DomainEvent;
