using BuildingBlocks.Core.CQRS.Event.Internal;
using NextGen.Modules.Inventories.Brands;
using NextGen.Modules.Inventories.Categories;
using NextGen.Modules.Inventories.Products.Models;
using NextGen.Modules.Inventories.Products.ValueObjects;
using NextGen.Modules.Inventories.Suppliers;

namespace NextGen.Modules.Inventories.Products.Features.CreatingProduct.Events.Domain;

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
