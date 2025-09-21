using BuildingBlocks.Core.Domain.Exceptions;

namespace NextGen.Modules.Inventorys.Suppliers.Exceptions.Domain;

public class SupplierDomainException : DomainException
{
    public SupplierDomainException(string message) : base(message)
    {
    }
}
