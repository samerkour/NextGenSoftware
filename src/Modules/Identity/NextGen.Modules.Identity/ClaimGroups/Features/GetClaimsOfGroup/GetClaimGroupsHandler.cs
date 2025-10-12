using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Query;
using Microsoft.EntityFrameworkCore;
using NextGen.Modules.Identity.ClaimGroups.Features.GetClaimGroups;
using NextGen.Modules.Identity.Shared.Data;

internal sealed class GetClaimGroupsHandler
    : IQueryHandler<GetClaimGroupsQuery, List<GetClaimGroupsResponse>>
{
    private readonly IdentityContext _db;
    private readonly IMapper _mapper;

    public GetClaimGroupsHandler(IdentityContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<List<GetClaimGroupsResponse>> Handle(GetClaimGroupsQuery request, CancellationToken cancellationToken)
    {
        var query = _db.ClaimGroups.AsNoTracking()
                                   .Where(cg => cg.DeletedOn == null);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            query = query.Where(cg => cg.Name.Contains(request.SearchTerm));

        var groups = await query.OrderBy(cg => cg.Name).ToListAsync(cancellationToken);

        return _mapper.Map<List<GetClaimGroupsResponse>>(groups);
    }
}
