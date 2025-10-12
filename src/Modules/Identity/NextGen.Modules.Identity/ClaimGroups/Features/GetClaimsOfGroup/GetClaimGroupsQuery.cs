using BuildingBlocks.Abstractions.CQRS.Query;
using System.Collections.Generic;

namespace NextGen.Modules.Identity.ClaimGroups.Features.GetClaimGroups
{
    public record GetClaimGroupsQuery(string? SearchTerm = null)
        : IQuery<List<GetClaimGroupsResponse>>;
}
