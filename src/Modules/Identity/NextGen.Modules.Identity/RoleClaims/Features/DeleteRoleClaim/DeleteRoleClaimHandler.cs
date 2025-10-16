using BuildingBlocks.Abstractions.CQRS.Command;
using Microsoft.EntityFrameworkCore;
using NextGen.Modules.Identity.Shared.Data;

namespace NextGen.Modules.Identity.RoleClaims.Features.DeleteRoleClaim
{
    internal sealed class DeleteRoleClaimHandler
        : ICommandHandler<DeleteRoleClaimCommand, bool>
    {
        private readonly IdentityContext _db;

        public DeleteRoleClaimHandler(IdentityContext db)
        {
            _db = db;
        }

        public async Task<bool> Handle(DeleteRoleClaimCommand request, CancellationToken cancellationToken)
        {
            var roleClaim = await _db.RoleClaims
                .FirstOrDefaultAsync(rc => rc.RoleId == request.RoleId && rc.ClaimId == request.ClaimId, cancellationToken);

            if (roleClaim == null)
                throw new KeyNotFoundException("Role claim not found.");

            if (request.IsDeleted)
            {
                roleClaim.DeletedOn = DateTime.UtcNow;
            }
            else
            {
                roleClaim.DeletedOn = null;
            }

            var result = await _db.SaveChangesAsync(cancellationToken);
            return result > 0;
        }
    }
}
