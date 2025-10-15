using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Command;
using Microsoft.EntityFrameworkCore;
using NextGen.Modules.Identity.Roles.Dtos;
using NextGen.Modules.Identity.Shared.Data;

namespace NextGen.Modules.Identity.Roles.Features.UpdateRole;

public class UpdateRoleHandler : ICommandHandler<UpdateRoleCommand, UpdateRoleResponse>
{
    private readonly IdentityContext _context;
    private readonly IMapper _mapper;

    public UpdateRoleHandler(IdentityContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UpdateRoleResponse> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _context.Roles
            .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

        if (role is null)
            return null!;

        role.Name = request.Name;
        role.Description = request.Description;
        role.UpdatedOn = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        var roleDto = _mapper.Map<RoleDto>(role);
        return new UpdateRoleResponse(roleDto);
    }
}
