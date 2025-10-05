using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.Domain;

namespace NextGen.Modules.Parties.Parties.ValueObjects;

public record PartyId : AggregateId
{
    public PartyId(long value) : base(value)
    {
        Guard.Against.NegativeOrZero(value, nameof(value));
    }

    public static implicit operator long(PartyId id) => Guard.Against.Null(id.Value, nameof(id.Value));

    public static implicit operator PartyId(long id) => new(id);
}
