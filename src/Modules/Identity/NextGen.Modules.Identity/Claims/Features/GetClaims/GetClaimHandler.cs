using BuildingBlocks.Abstractions.CQRS.Query;
using Microsoft.EntityFrameworkCore;
using NextGen.Modules.Identity.Shared.Data;

namespace NextGen.Modules.Identity.Claims.Features.GetClaims
{
    internal sealed class Handler : IQueryHandler<GetClaimsQuery, List<Response>>
    {
        private readonly IdentityContext _db;

        public Handler(IdentityContext db) => _db = db;

        public async Task<List<Response>> Handle(GetClaimsQuery request, CancellationToken cancellationToken)
        {
            return await _db.Claims
                .Select(c => new Response
                {
                    Id = c.Id,
                    Type = c.Type,
                    Value = c.Value,
                    ClaimGroupId = c.ClaimGroupId,
                    CreatedAt = c.CreatedAt
                })
                .ToListAsync(cancellationToken);
        }
    }
}
