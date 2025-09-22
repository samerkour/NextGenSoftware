using Ardalis.GuardClauses;
using BuildingBlocks.Core.Exception;
using NextGen.Modules.Inventories.Products.Exceptions.Domain;
using NextGen.Modules.Inventories.Suppliers;

namespace NextGen.Modules.Inventories.Products.ValueObjects;

public record SupplierInformation
{
    public SupplierInformation Null => null;

    public Name Name { get; }
    public SupplierId Id { get; }

    public SupplierInformation(SupplierId id, Name name)
    {
        Id = Guard.Against.Null(id, new ProductDomainException("SupplierId can not be null."));
        Name = Guard.Against.Null(name, new ProductDomainException("Name cannot be null."));
    }
}
