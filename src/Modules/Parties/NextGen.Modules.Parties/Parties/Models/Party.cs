using Ardalis.GuardClauses;
using BuildingBlocks.Core.Domain;
using BuildingBlocks.Core.Domain.ValueObjects;
using NextGen.Modules.Parties.Parties.Features.CreatingParty.Events.Domain;
using NextGen.Modules.Parties.Parties.ValueObjects;

namespace NextGen.Modules.Parties.Parties.Models;

public class Party : Aggregate<PartyId>
{
    public Guid IdentityId { get; private set; }
    public Email Email { get; private set; } = null!;
    public PartyName Name { get; private set; } = null!;
    public Address? Address { get; private set; }
    public Nationality? Nationality { get; private set; }
    public BirthDate? BirthDate { get; private set; }
    public PhoneNumber? PhoneNumber { get; private set; }

    public static Party Create(PartyId id, Email email, PartyName name, Guid identityId)
    {
        var party = new Party
        {
            Id = Guard.Against.Null(id, nameof(id)),
            Email = Guard.Against.Null(email, nameof(email)),
            Name = Guard.Against.Null(name, nameof(name)),
            IdentityId = Guard.Against.NullOrEmpty(identityId, nameof(IdentityId)),
        };

        party.AddDomainEvents(new PartyCreated(party));

        return party;
    }
}
