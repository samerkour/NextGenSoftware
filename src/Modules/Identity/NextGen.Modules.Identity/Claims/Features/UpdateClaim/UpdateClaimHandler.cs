using BuildingBlocks.Abstractions.CQRS.Command;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NextGen.Modules.Identity.Identity.Data;
using NextGen.Modules.Identity.Shared.Data;

namespace NextGen.Modules.Identity.Claims.Features.UpdateClaim;

public class UpdateClaimHandler : ICommandHandler<UpdateClaimCommand, UpdateClaimResponse>
{
    private readonly IdentityContext _context;

    public UpdateClaimHandler(IdentityContext context)
    {
        _context = context;
    }

    public async Task<UpdateClaimResponse> Handle(UpdateClaimCommand command, CancellationToken cancellationToken)
    {
        var claim = await _context.Claims
            .FirstOrDefaultAsync(c => c.Id == command.Id, cancellationToken);

        if (claim is null)
            return null!; // در endpoint 404 بررسی می‌شود

        claim.Type = command.Type;
        claim.Value = command.Value;
        claim.ClaimGroupId = command.ClaimGroupId ??
            throw new ArgumentException("ClaimGroupId cannot be null");


        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateClaimResponse
        {
            Id = claim.Id,
            Type = claim.Type,
            Value = claim.Value,
            ClaimGroupId = claim.ClaimGroupId
        };
    }
}
