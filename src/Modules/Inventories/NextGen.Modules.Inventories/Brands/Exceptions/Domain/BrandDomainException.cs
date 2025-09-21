using BuildingBlocks.Core.Domain.Exceptions;

namespace NextGen.Modules.Inventorys.Brands.Exceptions.Domain;

public class BrandDomainException : DomainException
{
    public BrandDomainException(string message) : base(message)
    {
    }
}
