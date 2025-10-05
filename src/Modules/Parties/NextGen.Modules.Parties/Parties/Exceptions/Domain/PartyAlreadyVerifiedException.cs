using BuildingBlocks.Core.Exception.Types;

namespace NextGen.Modules.Parties.Parties.Exceptions.Domain;

internal class PartyAlreadyVerifiedException : AppException
{
    public long PartyId { get; }

    public PartyAlreadyVerifiedException(long partyId)
        : base($"Party with Id: '{partyId}' already verified.")
    {
        PartyId = partyId;
    }
}
