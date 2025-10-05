using BuildingBlocks.Core.Domain.Exceptions;

namespace NextGen.Modules.Parties.RestockSubscriptions.Exceptions.Domain;

public class RestockSubscriptionDomainException : DomainException
{
    public RestockSubscriptionDomainException(string message) : base(message)
    {
    }
}
