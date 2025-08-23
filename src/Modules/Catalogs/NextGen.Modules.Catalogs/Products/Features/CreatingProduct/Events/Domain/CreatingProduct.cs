using BuildingBlocks.Core.CQRS.Event.Internal;
using NextGen.Modules.Catalogs.Brands;
using NextGen.Modules.Catalogs.Categories;
using NextGen.Modules.Catalogs.Products.Models;
using NextGen.Modules.Catalogs.Products.ValueObjects;
using NextGen.Modules.Catalogs.Suppliers;

namespace NextGen.Modules.Catalogs.Products.Features.CreatingProduct.Events.Domain;

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
