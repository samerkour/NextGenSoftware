using BuildingBlocks.Abstractions.CQRS.Command;
using Microsoft.EntityFrameworkCore;
using NextGen.Modules.Identity.Shared.Data;

namespace NextGen.Modules.Identity.Roles.Features.DeleteRole
{
    public class DeleteRoleHandler : ICommandHandler<DeleteRoleCommand, bool>
    {
        private readonly IdentityContext _db;

        public DeleteRoleHandler(IdentityContext db)
        {
            _db = db;
        }

        public async Task<bool> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await _db.Roles.FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

            if (role == null)
                return false;

            if (request.IsDeleted)
            {
                role.DeletedOn = DateTime.UtcNow;
            }
            else
            {
                role.DeletedOn = null;
            }

            var result = await _db.SaveChangesAsync(cancellationToken);
            return result > 0;
        }
    }
}
