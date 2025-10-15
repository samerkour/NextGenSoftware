using BuildingBlocks.Abstractions.CQRS.Query;
using Microsoft.EntityFrameworkCore;
using NextGen.Modules.Identity.Shared.Data;

namespace NextGen.Modules.Identity.Roles.Features.GetRoleClaimGroups
{
    internal sealed class GetRoleClaimGroupsHandler : IQueryHandler<GetRoleClaimGroupsQuery, IEnumerable<GetRoleClaimGroupsResponse>>
    {
        private readonly IdentityContext _db;

        public GetRoleClaimGroupsHandler(IdentityContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<GetRoleClaimGroupsResponse>> Handle(GetRoleClaimGroupsQuery request, CancellationToken cancellationToken)
        {
            var claimGroups = await _db.RoleClaimGroups
                .AsNoTracking()
                .Where(rcg => rcg.RoleId == request.RoleId)
                .Include(rcg => rcg.ClaimGroup)
                .Select(rcg => new GetRoleClaimGroupsResponse
                {
                    Id = rcg.ClaimGroup.Id,
                    Name = rcg.ClaimGroup.Name!,
                    Description = rcg.ClaimGroup.Description
                })
                .ToListAsync(cancellationToken);

            return claimGroups;
        }
    }
}
