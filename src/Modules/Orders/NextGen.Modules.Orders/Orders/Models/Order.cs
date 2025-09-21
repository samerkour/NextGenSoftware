using BuildingBlocks.Core.Domain;
using NextGen.Modules.Orders.Orders.ValueObjects;

namespace NextGen.Modules.Orders.Orders.Models;

public class Order : Aggregate<OrderId>
{
    public PartyInfo Party { get; private set; } = null!;
    public ProductInfo Product { get; private set; } = null!;

    public static Order Create(PartyInfo partyInfo, ProductInfo productInfo)
    {
        //TODO: Complete order domain model
        return new Order
        {
            Party = partyInfo,
            Product = productInfo
        };
    }
}
