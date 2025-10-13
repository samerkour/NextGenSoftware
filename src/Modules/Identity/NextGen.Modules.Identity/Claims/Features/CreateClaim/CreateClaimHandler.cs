using System.Security.Claims;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Command;
using Microsoft.EntityFrameworkCore;
using NextGen.Modules.Identity.Claims.Dtos;
using NextGen.Modules.Identity.Claims.Features.CreateClaim;
using NextGen.Modules.Identity.Claims.Features.GetClaimById;
using NextGen.Modules.Identity.Shared.Data;
using NextGen.Modules.Identity.Shared.Models;

internal sealed class CreateClaimHandler : ICommandHandler<CreateClaimCommand, CreateClaimResponse>
{
    private readonly IdentityContext _db;
    private readonly IMapper _mapper;

    public CreateClaimHandler(IdentityContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<CreateClaimResponse> Handle(CreateClaimCommand request, CancellationToken cancellationToken)
    {
        var claimGroupExists = await _db.ClaimGroups
            .AnyAsync(cg => cg.Id == request.ClaimGroupId, cancellationToken);

        if (!claimGroupExists)
            throw new Exception($"ClaimGroup with Id {request.ClaimGroupId} does not exist.");

        var claim = new ApplicationClaim
        {
            Id = Guid.NewGuid(),
            Type = request.Type,
            Value = request.Value,
            Name = request.Name,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow
        };

        _db.Claims.Add(claim);
        await _db.SaveChangesAsync(cancellationToken);

        var claimDto = _mapper.Map<ClaimDto>(claim);

        return new CreateClaimResponse(claimDto);
    }

}
