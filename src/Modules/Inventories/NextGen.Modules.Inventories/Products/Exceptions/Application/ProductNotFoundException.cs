using BuildingBlocks.Core.Exception.Types;

namespace NextGen.Modules.Inventories.Products.Exceptions.Application;

public class ProductNotFoundException : NotFoundException
{
    public ProductNotFoundException(long id) : base($"Product with id '{id}' not found")
    {
    }

    public ProductNotFoundException(string message) : base(message)
    {
    }
}
