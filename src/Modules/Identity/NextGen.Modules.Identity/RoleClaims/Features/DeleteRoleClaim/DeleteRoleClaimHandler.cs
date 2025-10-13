using BuildingBlocks.Abstractions.CQRS.Command;
using Microsoft.EntityFrameworkCore;
using NextGen.Modules.Identity.Shared.Data;

namespace NextGen.Modules.Identity.RoleClaims.Features.DeleteRoleClaim
{
    internal sealed class DeleteRoleClaimHandler
        : ICommandHandler<DeleteRoleClaimCommand, DeleteRoleClaimResponse>
    {
        private readonly IdentityContext _db;

        public DeleteRoleClaimHandler(IdentityContext db)
        {
            _db = db;
        }

        public async Task<DeleteRoleClaimResponse> Handle(DeleteRoleClaimCommand request, CancellationToken cancellationToken)
        {
            var roleClaim = await _db.RoleClaims
                .FirstOrDefaultAsync(rc => rc.RoleId == request.RoleId && rc.ClaimId == request.ClaimId, cancellationToken);

            if (roleClaim == null)
                throw new KeyNotFoundException("Role claim not found.");

            _db.RoleClaims.Remove(roleClaim);
            await _db.SaveChangesAsync(cancellationToken);

            return new DeleteRoleClaimResponse
            {
                RoleId = request.RoleId,
                ClaimId = request.ClaimId,
                Message = "Role claim deleted successfully."
            };
        }
    }
}
