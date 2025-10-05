using BuildingBlocks.Core.Exception.Types;

namespace NextGen.Modules.Parties.RestockSubscriptions.Exceptions.Application;

public class RestockSubscriptionNotFoundException : NotFoundException
{
    public RestockSubscriptionNotFoundException(long id) : base("RestockSubscription with id: " + id + " not found")
    {
    }
}
