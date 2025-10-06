using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Query;
using BuildingBlocks.Core.CQRS.Query;
using Microsoft.EntityFrameworkCore;
using NextGen.Modules.Identity.Claims.Dtos;
using NextGen.Modules.Identity.Shared.Data;
using NextGen.Modules.Identity.Shared.Extensions;
using NextGen.Modules.Identity.Users.Dtos;
using NextGen.Modules.Identity.Users.Features.GettingUsers;

namespace NextGen.Modules.Identity.Claims.Features.GetClaims
{
    internal sealed class Handler : IQueryHandler<GetClaimsQuery, GetClaimResponse>
    {
        private readonly IdentityContext _db;
        private readonly IMapper _mapper;

        public Handler(IdentityContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<GetClaimResponse> Handle(GetClaimsQuery request, CancellationToken cancellationToken)
        {
            ListResultModel<ClaimDto> claims = await _db.FindAllClaimsByPageAsync<ClaimDto>(_mapper, request, cancellationToken);

            return new GetClaimResponse(claims);
        }
    }
}
