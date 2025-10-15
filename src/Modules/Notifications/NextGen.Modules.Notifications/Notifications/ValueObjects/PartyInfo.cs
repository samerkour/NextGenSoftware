using BuildingBlocks.Core.Domain;

namespace NextGen.Modules.Notifications.Notifications.ValueObjects;

public class PartyInfo : ValueObject
{
    public string Name { get; private set; }
    public long PartyId { get; private set; }

    public static PartyInfo Create(string name, long partyId)
    {
        return new PartyInfo { Name = name, PartyId = partyId };
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return PartyId;
    }
}
