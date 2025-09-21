using BuildingBlocks.Core.Domain.Exceptions;

namespace NextGen.Modules.Inventorys.Products.Exceptions.Domain;

public class MaxStockThresholdReachedException : DomainException
{
    public MaxStockThresholdReachedException(string message) : base(message)
    {
    }
}
