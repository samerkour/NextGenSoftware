using BuildingBlocks.Core.Domain;
using NextGen.Modules.Sales.Sales.ValueObjects;

namespace NextGen.Modules.Sales.Sales.Models;

public class Order : Aggregate<OrderId>
{
    public PartyInfo Party { get; private set; } = null!;
    public ProductInfo Product { get; private set; } = null!;

    public static Order Create(PartyInfo partyInfo, ProductInfo productInfo)
    {
        //TODO: Complete sale domain model
        return new Order
        {
            Party = partyInfo,
            Product = productInfo
        };
    }
}
