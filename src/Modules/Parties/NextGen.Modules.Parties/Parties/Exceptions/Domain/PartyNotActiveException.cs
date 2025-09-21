using BuildingBlocks.Core.Exception.Types;

namespace NextGen.Modules.Parties.Parties.Exceptions.Domain;

internal class PartyNotActiveException : AppException
{
    public long PartyId { get; }

    public PartyNotActiveException(long partyId) : base($"Party with ID: '{partyId}' is not active.")
    {
        PartyId = partyId;
    }
}
