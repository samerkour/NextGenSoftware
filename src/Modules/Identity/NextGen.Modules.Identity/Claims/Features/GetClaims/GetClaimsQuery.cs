using BuildingBlocks.Abstractions.CQRS.Query;

namespace NextGen.Modules.Identity.Claims.Features.GetClaims
{
    public record GetClaimsQuery
        () : IQuery<List<Response>>;
}
