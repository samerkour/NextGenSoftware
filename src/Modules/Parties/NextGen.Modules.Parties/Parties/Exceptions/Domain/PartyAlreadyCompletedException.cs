using BuildingBlocks.Core.Exception.Types;

namespace NextGen.Modules.Parties.Parties.Exceptions.Domain;

internal class PartyAlreadyCompletedException : AppException
{
    public long PartyId { get; }

    public PartyAlreadyCompletedException(long partyId)
        : base($"Party with ID: '{partyId}' already completed.")
    {
        PartyId = partyId;
    }
}
