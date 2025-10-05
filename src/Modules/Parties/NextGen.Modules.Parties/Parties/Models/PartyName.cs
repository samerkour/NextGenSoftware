using BuildingBlocks.Core.Domain;
using NextGen.Modules.Parties.Parties.Exceptions.Domain;

namespace NextGen.Modules.Parties.Parties.Models;

public class PartyName : ValueObject
{
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set;} = null!;
    public string FullName => FirstName + " " + LastName;

    public static readonly PartyName Empty = new();
    public static readonly PartyName? Null = null;

    public static PartyName Create(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName) || firstName.Length is > 100 or < 3)
        {
            throw new InvalidNameException(firstName ?? "null");
        }

        if (string.IsNullOrWhiteSpace(lastName) || lastName.Length is > 100 or < 3)
        {
            throw new InvalidNameException(lastName ?? "null");
        }

        return new PartyName
        {
            FirstName = firstName,
            LastName = lastName,
        };
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
    }
}
