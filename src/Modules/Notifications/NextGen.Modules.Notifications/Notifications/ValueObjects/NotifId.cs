using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.Domain;

namespace NextGen.Modules.Notifications.Notifications.ValueObjects;

public record NotifId : AggregateId
{
    public NotifId(long value) : base(value)
    {
        Guard.Against.NegativeOrZero(value, nameof(value));
    }

    public static implicit operator long(NotifId id) => Guard.Against.Null(id.Value, nameof(id.Value));

    public static implicit operator NotifId(long id) => new(id);
}
