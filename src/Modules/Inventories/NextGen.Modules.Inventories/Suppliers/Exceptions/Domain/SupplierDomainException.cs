using BuildingBlocks.Core.Domain.Exceptions;

namespace NextGen.Modules.Inventories.Suppliers.Exceptions.Domain;

public class SupplierDomainException : DomainException
{
    public SupplierDomainException(string message) : base(message)
    {
    }
}
