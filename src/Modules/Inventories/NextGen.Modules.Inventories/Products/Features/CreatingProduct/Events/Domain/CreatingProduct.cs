using BuildingBlocks.Core.CQRS.Event.Internal;
using NextGen.Modules.Inventorys.Brands;
using NextGen.Modules.Inventorys.Categories;
using NextGen.Modules.Inventorys.Products.Models;
using NextGen.Modules.Inventorys.Products.ValueObjects;
using NextGen.Modules.Inventorys.Suppliers;

namespace NextGen.Modules.Inventorys.Products.Features.CreatingProduct.Events.Domain;

public record CreatingProduct(
    ProductId Id,
    Name Name,
    Price Price,
    Stock Stock,
    ProductStatus Status,
    Dimensions Dimensions,
    Category? Category,
    Supplier? Supplier,
    Brand? Brand,
    string? Description = null) : DomainEvent;
