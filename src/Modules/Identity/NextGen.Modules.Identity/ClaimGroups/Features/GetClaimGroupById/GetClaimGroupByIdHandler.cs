using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Query;
using Microsoft.EntityFrameworkCore;
using NextGen.Modules.Identity.ClaimGroups.Features.GetClaimGroupById;
using NextGen.Modules.Identity.Shared.Data;
using System.Threading;
using System.Threading.Tasks;

internal sealed class GetClaimGroupByIdHandler
    : IQueryHandler<GetClaimGroupByIdQuery, GetClaimGroupByIdResponse?>
{
    private readonly IdentityContext _db;
    private readonly IMapper _mapper;

    public GetClaimGroupByIdHandler(IdentityContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<GetClaimGroupByIdResponse?> Handle(GetClaimGroupByIdQuery request, CancellationToken cancellationToken)
    {
        var group = await _db.ClaimGroups
            .AsNoTracking()
            .FirstOrDefaultAsync(cg => cg.Id == request.Id && cg.DeletedOn == null, cancellationToken);

        if (group == null)
            return null;

        return _mapper.Map<GetClaimGroupByIdResponse>(group);
    }
}
