using BuildingBlocks.Abstractions.CQRS.Command;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NextGen.Modules.Identity.Identity.Data;
using NextGen.Modules.Identity.Shared.Data;

namespace NextGen.Modules.Identity.Claims.Features.DeleteClaim;

public class DeleteClaimHandler : ICommandHandler<DeleteClaimCommand, DeleteClaimResponse>
{
    private readonly IdentityContext _context;

    public DeleteClaimHandler(IdentityContext context)
    {
        _context = context;
    }

    public async Task<DeleteClaimResponse> Handle(DeleteClaimCommand request, CancellationToken cancellationToken)
    {
        var claim = await _context.Claims
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (claim is null)
        {
            return new DeleteClaimResponse
            {
                Id = request.Id,
                IsDeleted = false
            };
        }

        _context.Claims.Remove(claim);
        await _context.SaveChangesAsync(cancellationToken);

        return new DeleteClaimResponse
        {
            Id = claim.Id,
            IsDeleted = true
        };
    }
}
