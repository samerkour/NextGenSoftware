using BuildingBlocks.Core.Domain.Exceptions;

namespace NextGen.Modules.Inventories.Products.Exceptions.Domain;

public class InsufficientStockException : DomainException
{
    public InsufficientStockException(string message) : base(message)
    {
    }
}
