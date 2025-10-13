using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Command;
using Microsoft.EntityFrameworkCore;
using NextGen.Modules.Identity.Claims.Dtos;
using NextGen.Modules.Identity.Claims.Features.CreateClaim;
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

    public async Task<UpdateClaimResponse> Handle(UpdateClaimCommand request, CancellationToken cancellationToken)
    {
        var claim = await _context.Claims
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (claim is null)
            return null!; // در endpoint 404 بررسی می‌شود

        // --- به روزرسانی مقادیر ---
        claim.Type = request.Type;
        claim.Value = request.Value;
        claim.Name = request.Name;
        claim.Description = request.Description;

        claim.UpdatedOn = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);


        var claimDto = _mapper.Map<ClaimDto>(claim);

        return new UpdateClaimResponse(claimDto);
    }
}
