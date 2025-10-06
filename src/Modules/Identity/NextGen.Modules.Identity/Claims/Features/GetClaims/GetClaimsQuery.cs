using BuildingBlocks.Abstractions.CQRS.Query;
using BuildingBlocks.Core.CQRS.Query;

namespace NextGen.Modules.Identity.Claims.Features.GetClaims
{
    public record GetClaimsQuery() : ListQuery<GetClaimResponse>;
}
