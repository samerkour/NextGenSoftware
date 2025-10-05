using BuildingBlocks.Abstractions.CQRS.Query;
using Microsoft.EntityFrameworkCore;
using NextGen.Modules.Identity.Shared.Data;

namespace NextGen.Modules.Identity.Claims.Features.GetClaimById
{
    internal sealed class GetClaimByIdHandler : IQueryHandler<GetClaimByIdQuery, GetClaimByIdResponse>
    {
        private readonly IdentityContext _db;

        public GetClaimByIdHandler(IdentityContext db) => _db = db;

        public async Task<GetClaimByIdResponse> Handle(GetClaimByIdQuery request, CancellationToken cancellationToken)
        {
            return await _db.Claims
                .Where(c => c.Id == request.Id)
                .Select(c => new GetClaimByIdResponse
                {
                    Id = c.Id,
                    Type = c.Type,
                    Value = c.Value,
                    ClaimGroupId = c.ClaimGroupId,
                    CreatedAt = c.CreatedAt
                })
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
