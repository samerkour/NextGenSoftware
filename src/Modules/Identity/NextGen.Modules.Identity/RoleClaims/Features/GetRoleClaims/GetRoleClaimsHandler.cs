using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Query;
using Microsoft.EntityFrameworkCore;
using NextGen.Modules.Identity.Shared.Data;

namespace NextGen.Modules.Identity.Roles.Features.GetRoleClaims
{
    internal sealed class GetRoleClaimsHandler : IQueryHandler<GetRoleClaimsQuery, IEnumerable<RoleClaimResponse>>
    {
        private readonly IdentityContext _db;
        private readonly IMapper _mapper;

        public GetRoleClaimsHandler(IdentityContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoleClaimResponse>> Handle(GetRoleClaimsQuery request, CancellationToken cancellationToken)
        {
            var claims = await _db.RoleClaims
                .Include(rc => rc.Claim)
                .Where(rc => rc.RoleId == request.RoleId)
                .ToListAsync(cancellationToken);

            return _mapper.Map<IEnumerable<RoleClaimResponse>>(claims);
        }

    }
}

