using BuildingBlocks.Core.Exception.Types;

namespace NextGen.Modules.Parties.Products.Exceptions;

public class ProductNotFoundException : NotFoundException
{
    public ProductNotFoundException(long id) : base($"Product with id {id} not found")
    {
    }
}
