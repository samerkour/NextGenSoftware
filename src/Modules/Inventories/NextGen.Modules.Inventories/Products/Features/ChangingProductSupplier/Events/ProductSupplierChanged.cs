using BuildingBlocks.Core.CQRS.Event.Internal;
using NextGen.Modules.Inventorys.Products.ValueObjects;
using NextGen.Modules.Inventorys.Suppliers;

namespace NextGen.Modules.Inventorys.Products.Features.ChangingProductSupplier.Events;

public record ProductSupplierChanged(SupplierId SupplierId, ProductId ProductId) : DomainEvent;
