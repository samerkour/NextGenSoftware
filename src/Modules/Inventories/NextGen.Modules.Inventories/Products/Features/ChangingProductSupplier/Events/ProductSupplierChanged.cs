using BuildingBlocks.Core.CQRS.Event.Internal;
using NextGen.Modules.Inventories.Products.ValueObjects;
using NextGen.Modules.Inventories.Suppliers;

namespace NextGen.Modules.Inventories.Products.Features.ChangingProductSupplier.Events;

public record ProductSupplierChanged(SupplierId SupplierId, ProductId ProductId) : DomainEvent;
