using BuildingBlocks.Core.CQRS.Event.Internal;
using NextGen.Modules.Catalogs.Brands;
using NextGen.Modules.Catalogs.Products.ValueObjects;

namespace NextGen.Modules.Catalogs.Products.Features.ChangingProductBrand.Events.Domain;

internal record ProductBrandChanged(BrandId BrandId, ProductId ProductId) : DomainEvent;
