using BuildingBlocks.Core.Domain.Exceptions;

namespace NextGen.Modules.Inventories.Brands.Exceptions.Domain;

public class BrandDomainException : DomainException
{
    public BrandDomainException(string message) : base(message)
    {
    }
}
