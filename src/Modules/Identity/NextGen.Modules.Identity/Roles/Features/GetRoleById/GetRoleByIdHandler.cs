using BuildingBlocks.Abstractions.CQRS.Query;
using Microsoft.EntityFrameworkCore;
using NextGen.Modules.Identity.Shared.Data;

namespace NextGen.Modules.Identity.Roles.Features.GetRoleById
{
    internal sealed class GetRoleByIdHandler : IQueryHandler<GetRoleByIdQuery, GetRoleByIdResponse>
    {
        private readonly IdentityContext _db;

        public GetRoleByIdHandler(IdentityContext db)
        {
            _db = db;
        }

        public async Task<GetRoleByIdResponse> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            var role = await _db.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == request.RoleId, cancellationToken);

            if (role == null)
                throw new KeyNotFoundException($"Role with Id '{request.RoleId}' not found.");

            return new GetRoleByIdResponse
            {
                Id = role.Id,
                Name = role.Name!,
                Description = role.Description
            };
        }
    }
}
