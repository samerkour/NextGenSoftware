using BuildingBlocks.Core.Domain.Exceptions;

namespace NextGen.Modules.Inventories.Categories.Exceptions.Domain;

public class CategoryDomainException : DomainException
{
    public CategoryDomainException(string message) : base(message)
    {
    }

    public CategoryDomainException(long id) : base($"Category with id: '{id}' not found.")
    {
    }
}
