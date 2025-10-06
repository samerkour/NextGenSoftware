// DeleteClaimHandler.cs
using BuildingBlocks.Abstractions.CQRS.Command;
using Microsoft.EntityFrameworkCore;
using NextGen.Modules.Identity.Shared.Data;

namespace NextGen.Modules.Identity.Claims.Features.DeleteClaim;

public class DeleteClaimHandler : ICommandHandler<DeleteClaimCommand, bool>
{
    private readonly IdentityContext _context;

    public DeleteClaimHandler(IdentityContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteClaimCommand request, CancellationToken cancellationToken)
    {
        var claim = await _context.Claims
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (claim == null)
            return false;

        if (request.IsDeleted)
        {
            claim.DeletedOn = DateTime.UtcNow;
        }
        else
        {
            claim.DeletedOn = null;
        }

        var result = await _context.SaveChangesAsync(cancellationToken);
        return result > 0;
    }
}
