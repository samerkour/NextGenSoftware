using BuildingBlocks.Core.Domain;

namespace NextGen.Modules.Inventories.Suppliers;

public class Supplier : Entity<SupplierId>
{
    public string Name { get; private set; }

    public Supplier(SupplierId id, string name) : base(id)
    {
        Name = name;
    }
}
