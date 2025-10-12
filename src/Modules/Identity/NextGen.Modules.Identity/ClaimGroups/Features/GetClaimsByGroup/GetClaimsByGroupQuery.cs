using BuildingBlocks.Abstractions.CQRS.Query;
using System.Collections.Generic;

namespace NextGen.Modules.Identity.ClaimGroups.Features.GetClaimsByGroup
{
    public record GetClaimsByGroupQuery(Guid GroupId)
        : IQuery<List<GetClaimsByGroupResponse>>;
}
