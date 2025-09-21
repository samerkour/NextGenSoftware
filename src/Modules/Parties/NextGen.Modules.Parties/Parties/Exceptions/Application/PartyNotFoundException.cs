using BuildingBlocks.Core.Exception.Types;

namespace NextGen.Modules.Parties.Parties.Exceptions.Application;

public class PartyNotFoundException : NotFoundException
{
    public PartyNotFoundException(string message) : base(message)
    {
    }

    public PartyNotFoundException(long id) : base($"Party with id '{id}' not found.")
    {
    }
}
