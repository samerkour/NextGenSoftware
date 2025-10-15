using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Query;
using NextGen.Modules.Identity.ClaimGroups.Dtos;
using NextGen.Modules.Identity.Shared.Data;
using NextGen.Modules.Identity.Shared.Extensions;

namespace NextGen.Modules.Identity.ClaimGroups.Features.GetClaimGroups;

internal sealed class GetClaimGroupsHandler : IQueryHandler<GetClaimGroupsQuery, GetClaimGroupsResponse>
{
    private readonly IdentityContext _db;
    private readonly IMapper _mapper;

    public GetClaimGroupsHandler(IdentityContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<GetClaimGroupsResponse> Handle(GetClaimGroupsQuery request, CancellationToken cancellationToken)
    {
        var groups = await _db.FindAllClaimGroupsByPageAsync<ClaimGroupDto>(_mapper, request, cancellationToken);
        return new GetClaimGroupsResponse(groups);
    }
}
