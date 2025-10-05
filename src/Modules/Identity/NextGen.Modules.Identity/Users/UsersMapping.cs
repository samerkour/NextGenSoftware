using AutoMapper;
using NextGen.Modules.Identity.Shared.Models;
using NextGen.Modules.Identity.Users.Dtos;

namespace NextGen.Modules.Identity.Users;

public class UsersMapping : Profile
{
    public UsersMapping()
    {
        CreateMap<ApplicationUser, IdentityUserDto>()
            .ForMember(x => x.IsDeleted, opt => opt.MapFrom(x => x.DeletedOn.HasValue == true ? true : false))
            .ForMember(x => x.IsLocked, opt => opt.MapFrom(x => x.LockoutEnd.HasValue == true ? true : false))
            .ForMember(x => x.IsTwoFactorEnabled, opt => opt.MapFrom(x => x.TwoFactorEnabled))
            .ForMember(
                x => x.Roles,
                opt => opt.MapFrom(x =>
                    x.UserRoles.Where(m => m.Role != null)
                        .Select(q => q.Role!.Name)));
    }
}
