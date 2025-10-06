using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Query;
using Microsoft.EntityFrameworkCore;
using NextGen.Modules.Identity.Claims.Features.GetClaimById;
using NextGen.Modules.Identity.Shared.Data;

internal sealed class GetClaimByIdHandler : IQueryHandler<GetClaimByIdQuery, GetClaimByIdResponse>
{
    private readonly IdentityContext _db;
    private readonly IMapper _mapper;

    public GetClaimByIdHandler(IdentityContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<GetClaimByIdResponse> Handle(GetClaimByIdQuery request, CancellationToken cancellationToken)
    {
        var claim = await _db.Claims
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (claim == null)
            return null!; 

        return _mapper.Map<GetClaimByIdResponse>(claim);
    }
}
