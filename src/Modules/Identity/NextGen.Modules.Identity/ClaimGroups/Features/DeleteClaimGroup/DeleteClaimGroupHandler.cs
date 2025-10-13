using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Command;
using Microsoft.EntityFrameworkCore;
using NextGen.Modules.Identity.Shared.Data;
using NextGen.Modules.Identity.Shared.Models;

namespace NextGen.Modules.Identity.ClaimGroups.Features.DeleteClaimGroup;

public class DeleteClaimGroupHandler
    : ICommandHandler<DeleteClaimGroupCommand, DeleteClaimGroupResponse>
{
    private readonly IdentityContext _db;
    private readonly IMapper _mapper;

    public DeleteClaimGroupHandler(IdentityContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<DeleteClaimGroupResponse> Handle(DeleteClaimGroupCommand request, CancellationToken cancellationToken)
    {
        var claimGroup = await _db.ClaimGroups
            .Include(g => g.ClaimGroupClaims)
            .FirstOrDefaultAsync(g => g.Id == request.GroupId, cancellationToken);

        if (claimGroup == null)
            throw new KeyNotFoundException("Claim group not found.");

        // soft delete / restore
        if (request.IsDeleted)
            claimGroup.DeletedOn = DateTime.UtcNow;
        else
            claimGroup.DeletedOn = null;

        await _db.SaveChangesAsync(cancellationToken);

        return new DeleteClaimGroupResponse
        {
            GroupId = request.GroupId,
            IsDeleted = request.IsDeleted,
            Message = request.IsDeleted
                ? "Claim group deleted successfully."
                : "Claim group restored successfully."
        };
    }
}
