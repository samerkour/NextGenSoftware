using BuildingBlocks.Core.Domain.Exceptions;

namespace NextGen.Modules.Inventorys.Products.Exceptions.Domain;

public class InsufficientStockException : DomainException
{
    public InsufficientStockException(string message) : base(message)
    {
    }
}
