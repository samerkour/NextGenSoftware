using BuildingBlocks.Core.CQRS.Event.Internal;
using NextGen.Modules.Catalogs.Products.ValueObjects;
using NextGen.Modules.Catalogs.Suppliers;

namespace NextGen.Modules.Catalogs.Products.Features.ChangingProductSupplier.Events;

public record ProductSupplierChanged(SupplierId SupplierId, ProductId ProductId) : DomainEvent;
