using BuildingBlocks.Core.Domain.Exceptions;

namespace NextGen.Modules.Inventorys.Products.Exceptions.Domain;

public class ProductDomainException : DomainException
{
    public ProductDomainException(string message) : base(message)
    {
    }
}
