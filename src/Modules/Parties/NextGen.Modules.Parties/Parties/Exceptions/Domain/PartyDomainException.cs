using System.Net;
using BuildingBlocks.Core.Domain.Exceptions;

namespace NextGen.Modules.Parties.Parties.Exceptions.Domain;

public class PartyDomainException : DomainException
{
    public PartyDomainException(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest) :
        base(message, statusCode)
    {
    }
}
