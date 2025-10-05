using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Query;
using BuildingBlocks.Core.CQRS.Query;
using BuildingBlocks.Core.Persistence.EfCore;
using BuildingBlocks.Core.Types;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NextGen.Modules.Identity.Shared.Extensions;
using NextGen.Modules.Identity.Shared.Models;
using NextGen.Modules.Identity.Users.Dtos;

namespace NextGen.Modules.Identity.Users.Features.GettingUsers;

public class GetUsersHandler : IQueryHandler<GetUsersQuery, GetUsersResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public GetUsersHandler(UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<GetUsersResponse> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        ListResultModel<IdentityUserDto> users = await _userManager.FindAllUsersByPageAsync<IdentityUserDto>(_mapper, request, cancellationToken);

        return new GetUsersResponse(users);
    }
}
