using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Command;
using Microsoft.EntityFrameworkCore;
using NextGen.Modules.Identity.Roles.Features.GetRoleClaims;
using NextGen.Modules.Identity.Shared.Data;
using NextGen.Modules.Identity.Shared.Models;

namespace NextGen.Modules.Identity.Roles.Features.AddClaimToRole
{
    internal sealed class AddClaimToRoleHandler : ICommandHandler<AddClaimToRoleCommand, IEnumerable<RoleClaimResponse>>
    {
        private readonly IdentityContext _db;
        private readonly IMapper _mapper;

        public AddClaimToRoleHandler(IdentityContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoleClaimResponse>> Handle(AddClaimToRoleCommand request, CancellationToken cancellationToken)
        {
            var exists = await _db.RoleClaims
                .AnyAsync(rc => rc.RoleId == request.RoleId && rc.ClaimId == request.ClaimId, cancellationToken);

            if (!exists)
            {
                var roleClaim = new RoleClaim
                {
                    RoleId = request.RoleId,
                    ClaimId = request.ClaimId,
                };

                _db.RoleClaims.Add(roleClaim);
                await _db.SaveChangesAsync(cancellationToken);
            }

            var claims = await _db.RoleClaims
                .Include(rc => rc.Claim)
                .Where(rc => rc.RoleId == request.RoleId)
                .ToListAsync(cancellationToken);

            return _mapper.Map<IEnumerable<RoleClaimResponse>>(claims);
        }
    }
}
