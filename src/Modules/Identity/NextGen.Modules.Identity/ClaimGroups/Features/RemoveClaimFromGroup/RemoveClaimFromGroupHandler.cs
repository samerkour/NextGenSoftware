using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Command;
using Microsoft.EntityFrameworkCore;
using NextGen.Modules.Identity.Shared.Data;
using NextGen.Modules.Identity.Shared.Models;

namespace NextGen.Modules.Identity.ClaimGroups.Features.RemoveClaimFromGroup
{
    public class RemoveClaimFromGroupHandler
        : ICommandHandler<RemoveClaimFromGroupRequest, RemoveClaimFromGroupResponse>
    {
        private readonly IdentityContext _db;

        public RemoveClaimFromGroupHandler(IdentityContext db)
        {
            _db = db;
        }

        public async Task<RemoveClaimFromGroupResponse> Handle(RemoveClaimFromGroupRequest request, CancellationToken cancellationToken)
        {
            var entity = await _db.ClaimGroupClaims
                .FirstOrDefaultAsync(x => x.ClaimGroupId == request.GroupId && x.ClaimId == request.ClaimId, cancellationToken);

            if (entity == null)
                return new RemoveClaimFromGroupResponse
                {
                    GroupId = request.GroupId,
                    ClaimId = request.ClaimId,
                    Removed = false
                };

            _db.ClaimGroupClaims.Remove(entity);
            await _db.SaveChangesAsync(cancellationToken);

            return new RemoveClaimFromGroupResponse
            {
                GroupId = request.GroupId,
                ClaimId = request.ClaimId,
                Removed = true
            };
        }
    }
}
