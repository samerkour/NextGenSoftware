using BuildingBlocks.Core.Exception.Types;

namespace NextGen.Modules.Inventories.Brands.Exceptions.Application;

public class BrandNotFoundException : NotFoundException
{
    public BrandNotFoundException(long id) : base($"Brand with id '{id}' not found")
    {
    }

    public BrandNotFoundException(string message) : base(message)
    {
    }
}
