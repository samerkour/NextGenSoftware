using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Command;
using NextGen.Modules.Identity.Shared.Data;
using NextGen.Modules.Identity.Shared.Models;

namespace NextGen.Modules.Identity.ClaimGroups.Features.CreateClaimGroup
{
    public class CreateClaimGroupHandler
        : ICommandHandler<CreateClaimGroupCommand, CreateClaimGroupResponse>
    {
        private readonly IdentityContext _db;
        private readonly IMapper _mapper;

        public CreateClaimGroupHandler(IdentityContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<CreateClaimGroupResponse> Handle(CreateClaimGroupCommand request, CancellationToken cancellationToken)
        {
            var claimGroup = new ClaimGroup
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                CreatedAt = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow
            };

            _db.ClaimGroups.Add(claimGroup);
            await _db.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CreateClaimGroupResponse>(claimGroup);
        }
    }
}
