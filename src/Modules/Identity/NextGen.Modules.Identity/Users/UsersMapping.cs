using AutoMapper;
using NextGen.Modules.Identity.Shared.Models;
using NextGen.Modules.Identity.Users.Dtos;

namespace NextGen.Modules.Identity.Users;

public class UsersMapping : Profile
{
    public UsersMapping()
    {
        CreateMap<ApplicationUser, IdentityUserDto>()
            .ForMember(x => x.RefreshTokens, opt => opt.MapFrom(x => x.RefreshTokens.Select(r => r.Token)))
            .ForMember(
                x => x.Roles,
                opt => opt.MapFrom(x =>
                    x.UserRoles.Where(m => m.Role != null)
                        .Select(q => q.Role!.Name)));
    }
}
