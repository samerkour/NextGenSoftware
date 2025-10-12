using BuildingBlocks.Abstractions.CQRS.Query;
using Microsoft.EntityFrameworkCore;
using NextGen.Modules.Identity.Shared.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NextGen.Modules.Identity.ClaimGroups.Features.GetClaimsByGroup
{
    public class GetClaimsByGroupHandler : IQueryHandler<GetClaimsByGroupQuery, List<GetClaimsByGroupResponse>>
    {
        private readonly IdentityContext _db;

        public GetClaimsByGroupHandler(IdentityContext db)
        {
            _db = db;
        }

        public async Task<List<GetClaimsByGroupResponse>> Handle(GetClaimsByGroupQuery request, CancellationToken cancellationToken)
        {
            var claims = await _db.Claims
                .Where(c => c.ClaimGroupId == request.GroupId && c.DeletedOn == null)
                .Select(c => new GetClaimsByGroupResponse
                {
                    ClaimId = c.Id,
                    Type = c.Type,
                    Value = c.Value
                })
                .ToListAsync(cancellationToken);

            return claims;
        }
    }
}
