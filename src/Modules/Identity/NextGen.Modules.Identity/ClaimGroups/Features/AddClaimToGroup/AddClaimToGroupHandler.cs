using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Command;
using Microsoft.EntityFrameworkCore;
using NextGen.Modules.Identity.Shared.Data;
using NextGen.Modules.Identity.Shared.Models;

namespace NextGen.Modules.Identity.ClaimGroups.Features.AddClaimToGroup
{
    public class AddClaimToGroupHandler
        : ICommandHandler<AddClaimToGroupCommand, AddClaimToGroupResponse>
    {
        private readonly IdentityContext _db;
        private readonly IMapper _mapper;

        public AddClaimToGroupHandler(IdentityContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<AddClaimToGroupResponse> Handle(AddClaimToGroupCommand request, CancellationToken cancellationToken)
        {
            var claimGroup = await _db.ClaimGroups
                .Include(g => g.ClaimGroupClaims)
                .FirstOrDefaultAsync(g => g.Id == request.GroupId, cancellationToken);

            if (claimGroup == null)
                throw new KeyNotFoundException("Claim group not found.");

            var claim = await _db.Claims
                .FirstOrDefaultAsync(c => c.Id == request.ClaimId, cancellationToken);

            if (claim == null)
                throw new KeyNotFoundException("Claim not found.");

            if (claimGroup.ClaimGroupClaims.Any(c => c.ClaimId == request.ClaimId))
                throw new InvalidOperationException("Claim already assigned to this group.");

            _db.ClaimGroupClaims.Add(new ClaimGroupClaim() { ClaimGroupId = request.GroupId, ClaimId=request.ClaimId });

            await _db.SaveChangesAsync(cancellationToken);

            return new AddClaimToGroupResponse
            {
                GroupId = claimGroup.Id,
                ClaimId = claim.Id
            };
        }
    }
}
