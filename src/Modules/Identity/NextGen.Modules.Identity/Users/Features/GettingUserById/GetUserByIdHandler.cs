using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Query;
using NextGen.Modules.Identity.Shared.Extensions;
using NextGen.Modules.Identity.Shared.Models;
using NextGen.Modules.Identity.Users.Dtos;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace NextGen.Modules.Identity.Users.Features.GettingUserById;

internal class GetUserByIdHandler : IQueryHandler<GetUserByIdQuery, UserByIdResponse>
{
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;

    public GetUserByIdHandler(UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        _mapper = mapper;
        _userManager = Guard.Against.Null(userManager, nameof(userManager));
    }

    public async Task<UserByIdResponse> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        Guard.Against.Null(query, nameof(query));

        ApplicationUser? identityUser = await _userManager.FindUserWithRoleByIdAsync(query.Id);

        IdentityUserDto identityUserDto = _mapper.Map<IdentityUserDto>(identityUser);

        return new UserByIdResponse(identityUserDto);
    }
}
