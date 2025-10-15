using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Query;
using Microsoft.EntityFrameworkCore;
using NextGen.Modules.Identity.Roles.Dtos;
using NextGen.Modules.Identity.Shared.Data;
using NextGen.Modules.Identity.Shared.Extensions;

namespace NextGen.Modules.Identity.Roles.Features.GetRoles;

internal sealed class GetRolesHandler : IQueryHandler<GetRolesQuery, GetRolesResponse>
{
    private readonly IdentityContext _db;
    private readonly IMapper _mapper;

    public GetRolesHandler(IdentityContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<GetRolesResponse> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await _db.FindAllRolesByPageAsync<RoleDto>(_mapper, request, cancellationToken);

        return new GetRolesResponse(roles);
    }
}
