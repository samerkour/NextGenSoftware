using BuildingBlocks.Abstractions.CQRS.Command;
using Microsoft.EntityFrameworkCore;
using NextGen.Modules.Identity.Shared.Data;

namespace NextGen.Modules.Identity.ClaimGroups.Features.RemoveClaimFromGroup
{
    public class RemoveClaimFromGroupHandler : ICommandHandler<RemoveClaimFromGroupCommand, bool>
    {
        private readonly IdentityContext _db;

        public RemoveClaimFromGroupHandler(IdentityContext db)
        {
            _db = db;
        }

        public async Task<bool> Handle(RemoveClaimFromGroupCommand request, CancellationToken cancellationToken)
        {
            var entity = await _db.ClaimGroupClaims
                .FirstOrDefaultAsync(x => x.ClaimGroupId == request.GroupId
                                       && x.ClaimId == request.ClaimId, cancellationToken);

            if (entity == null)
                return false;

            entity.DeletedOn = request.IsDeleted ? DateTime.UtcNow : null;

            _db.ClaimGroupClaims.Update(entity);
            var result = await _db.SaveChangesAsync(cancellationToken);
            return result > 0;
        }
    }
}
