using BuildingBlocks.Core.Domain.Exceptions;

namespace NextGen.Modules.Inventories.Products.Exceptions.Domain;

public class ProductDomainException : DomainException
{
    public ProductDomainException(string message) : base(message)
    {
    }
}
