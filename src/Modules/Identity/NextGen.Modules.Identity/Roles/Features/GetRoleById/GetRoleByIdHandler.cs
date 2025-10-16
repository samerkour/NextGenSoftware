using System.Security.Claims;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Query;
using BuildingBlocks.Core.Mapping;
using Microsoft.EntityFrameworkCore;
using NextGen.Modules.Identity.Claims.Dtos;
using NextGen.Modules.Identity.Claims.Features.GetClaimById;
using NextGen.Modules.Identity.Roles.Dtos;
using NextGen.Modules.Identity.Shared.Data;

namespace NextGen.Modules.Identity.Roles.Features.GetRoleById
{
    internal sealed class GetRoleByIdHandler : IQueryHandler<GetRoleByIdQuery, GetRoleByIdResponse>
    {
        private readonly IdentityContext _db;
        private readonly IMapper _mapper;

        public GetRoleByIdHandler(IdentityContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<GetRoleByIdResponse> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            var role = await _db.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == request.RoleId, cancellationToken);

            if (role == null)
                return null!;

            var claimDto = _mapper.Map<RoleDto>(role);

            return new GetRoleByIdResponse(claimDto);
        }
    }
}
