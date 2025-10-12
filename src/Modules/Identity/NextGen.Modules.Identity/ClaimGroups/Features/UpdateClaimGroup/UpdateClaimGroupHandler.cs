using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Command;
using Microsoft.EntityFrameworkCore;
using NextGen.Modules.Identity.Shared.Data;
using NextGen.Modules.Identity.Shared.Models;

namespace NextGen.Modules.Identity.ClaimGroups.Features.UpdateClaimGroup
{
    public class UpdateClaimGroupHandler
        : ICommandHandler<UpdateClaimGroupCommand, UpdateClaimGroupResponse>
    {
        private readonly IdentityContext _db;
        private readonly IMapper _mapper;

        public UpdateClaimGroupHandler(IdentityContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<UpdateClaimGroupResponse> Handle(UpdateClaimGroupCommand request, CancellationToken cancellationToken)
        {
            var claimGroup = await _db.ClaimGroups
                .FirstOrDefaultAsync(cg => cg.Id == request.Id, cancellationToken);

            if (claimGroup == null)
                throw new KeyNotFoundException($"Claim group with ID '{request.Id}' not found.");

            // فقط این فیلدها ویرایش می‌شوند
            claimGroup.Name = request.Name;
            claimGroup.Description = request.Description;
            claimGroup.UpdatedOn = DateTime.UtcNow;

            await _db.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UpdateClaimGroupResponse>(claimGroup);
        }
    }
}
