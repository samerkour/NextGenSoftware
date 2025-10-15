using BuildingBlocks.Abstractions.CQRS.Command;
using Microsoft.EntityFrameworkCore;
using NextGen.Modules.Identity.Shared.Data;
using NextGen.Modules.Identity.Shared.Models;

namespace NextGen.Modules.Identity.Roles.Features.CreateRole
{
    internal sealed class CreateRoleHandler : ICommandHandler<CreateRoleCommand, CreateRoleResponse>
    {
        private readonly IdentityContext _db;

        public CreateRoleHandler(IdentityContext db)
        {
            _db = db;
        }

        public async Task<CreateRoleResponse> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            // بررسی تکراری بودن
            var exists = await _db.Roles.AnyAsync(r => r.Name == request.Name, cancellationToken);
            if (exists)
                throw new InvalidOperationException($"Role with name '{request.Name}' already exists.");

            var role = new Role
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                NormalizedName = request.Name.ToUpperInvariant(),
                Description = request.Description,
                CreatedAt = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow
            };

            _db.Roles.Add(role);
            await _db.SaveChangesAsync(cancellationToken);

            return new CreateRoleResponse
            {
                Id = role.Id,
                Name = role.Name!,
                Description = role.Description,
                Message = "Role created successfully."
            };
        }
    }
}
