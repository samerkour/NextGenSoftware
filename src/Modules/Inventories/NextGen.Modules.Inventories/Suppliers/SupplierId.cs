using BuildingBlocks.Abstractions.Domain;

namespace NextGen.Modules.Inventories.Suppliers;

public record SupplierId : AggregateId
{
    public SupplierId(long value) : base(value)
    {
    }

    public static implicit operator long(SupplierId id) => id.Value;

    public static implicit operator SupplierId(long id) => new(id);
}
