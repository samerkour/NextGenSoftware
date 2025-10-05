namespace NextGen.Modules.Parties.Parties.Features.CreatingParty;

public record CreatePartyResponse(
    long PartyId,
    string Email,
    string FirstName,
    string LastName,
    Guid IdentityUserId);
