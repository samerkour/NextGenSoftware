using BuildingBlocks.Core.Exception.Types;

namespace NextGen.Modules.Parties.Parties.Exceptions.Application;

internal class PartyAlreadyExistsException : AppException
{
    public long? PartyId { get; }
    public Guid? IdentityId { get; }

    public PartyAlreadyExistsException(string message) : base(message)
    {
    }

    public PartyAlreadyExistsException(Guid identityId)
        : base($"Party with IdentityId: '{identityId}' already exists.")
    {
        IdentityId = identityId;
    }

    public PartyAlreadyExistsException(long partyId)
        : base($"Party with ID: '{partyId}' already exists.")
    {
        PartyId = partyId;
    }
}
