using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Command;
using Microsoft.EntityFrameworkCore;
using NextGen.Modules.Identity.Shared.Data;

namespace NextGen.Modules.Identity.Claims.Features.UpdateClaim;

public class UpdateClaimHandler : ICommandHandler<UpdateClaimCommand, UpdateClaimResponse>
{
    private readonly IdentityContext _context;
    private readonly IMapper _mapper;

    public UpdateClaimHandler(IdentityContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UpdateClaimResponse> Handle(UpdateClaimCommand command, CancellationToken cancellationToken)
    {
        var claim = await _context.Claims
            .FirstOrDefaultAsync(c => c.Id == command.Id, cancellationToken);

        if (claim is null)
            return null!; // در endpoint 404 بررسی می‌شود

        // --- به روزرسانی مقادیر ---
        claim.Type = command.Type;
        claim.Value = command.Value;
        claim.ClaimGroupId = command.ClaimGroupId;

        claim.UpdatedOn = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        // --- تبدیل به Response با AutoMapper ---
        return _mapper.Map<UpdateClaimResponse>(claim);
    }
}
