using BuildingBlocks.Core.Domain;
using NextGen.Modules.Notifications.Notifications.ValueObjects;

namespace NextGen.Modules.Notifications.Notifications.Models;

public class Notif : Aggregate<NotifId>
{
    public PartyInfo Party { get; private set; } = null!;
    public ProductInfo Product { get; private set; } = null!;

    public static Notif Create(PartyInfo partyInfo, ProductInfo productInfo)
    {
        //TODO: Complete notification domain model
        return new Notif
        {
            Party = partyInfo,
            Product = productInfo
        };
    }
}
