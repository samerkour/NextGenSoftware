using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Command;
using Microsoft.EntityFrameworkCore;
using NextGen.Modules.Identity.Roles.Features.AssignClaimGroupToRole;
using NextGen.Modules.Identity.Shared.Data;
using NextGen.Modules.Identity.Shared.Models;

namespace NextGen.Modules.Identity.ClaimGroups.Features.AssignClaimGroupToRole
{
    public class AssignClaimGroupToRoleHandler
        : ICommandHandler<AssignClaimGroupToRoleCommand, AssignClaimGroupToRoleResponse>
    {
        private readonly IdentityContext _db;

        public AssignClaimGroupToRoleHandler(IdentityContext db)
        {
            _db = db;
        }

        public async Task<AssignClaimGroupToRoleResponse> Handle(AssignClaimGroupToRoleCommand request, CancellationToken cancellationToken)
        {
            var claimGroup = await _db.ClaimGroups
                //.Include(cg => cg.Roles)
                .FirstOrDefaultAsync(cg => cg.Id == request.ClaimGroupId, cancellationToken);

            if (claimGroup == null)
                throw new KeyNotFoundException("ClaimGroup not found");

            var role = await _db.Roles.FirstOrDefaultAsync(r => r.Id == request.RoleId, cancellationToken);

            if (role == null)
                throw new KeyNotFoundException("Role not found");

            //// ✅ اضافه کردن نقش به ClaimGroup
            //if (!claimGroup.Roles.Contains(role))
            //{
            //   // claimGroup.Roles.Add(role);
            //    await _db.SaveChangesAsync(cancellationToken);
            //}

            return new AssignClaimGroupToRoleResponse
            {
                ClaimGroupId = claimGroup.Id,
                RoleId = role.Id
            };
        }
    }
}
