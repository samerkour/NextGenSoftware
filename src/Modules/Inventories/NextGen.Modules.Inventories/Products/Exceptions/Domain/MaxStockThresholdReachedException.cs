using BuildingBlocks.Core.Domain.Exceptions;

namespace NextGen.Modules.Inventories.Products.Exceptions.Domain;

public class MaxStockThresholdReachedException : DomainException
{
    public MaxStockThresholdReachedException(string message) : base(message)
    {
    }
}
