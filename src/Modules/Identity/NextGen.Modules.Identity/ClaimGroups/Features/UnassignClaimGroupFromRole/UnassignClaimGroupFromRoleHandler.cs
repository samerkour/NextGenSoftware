using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Command;
using Microsoft.EntityFrameworkCore;
using NextGen.Modules.Identity.Shared.Data;
using NextGen.Modules.Identity.Shared.Models;

namespace NextGen.Modules.Identity.ClaimGroups.Features.UnassignClaimGroupFromRole
{
    public class UnassignClaimGroupFromRoleHandler
        : ICommandHandler<UnassignClaimGroupFromRoleCommand, UnassignClaimGroupFromRoleResponse>
    {
        private readonly IdentityContext _db;
        private readonly IMapper _mapper;

        public UnassignClaimGroupFromRoleHandler(IdentityContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<UnassignClaimGroupFromRoleResponse> Handle(UnassignClaimGroupFromRoleCommand request, CancellationToken cancellationToken)
        {
            var entity = await _db.RoleClaimGroups
                .FirstOrDefaultAsync(x => x.ClaimGroupId == request.ClaimGroupId && x.RoleId == request.RoleId, cancellationToken);

            if (entity != null)
            {
                _db.RoleClaimGroups.Remove(entity);
                await _db.SaveChangesAsync(cancellationToken);
            }

            return new UnassignClaimGroupFromRoleResponse
            {
                ClaimGroupId = request.ClaimGroupId,
                RoleId = request.RoleId,
                Success = entity != null
            };
        }
    }
}
