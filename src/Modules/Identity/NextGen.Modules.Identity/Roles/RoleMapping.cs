using AutoMapper;
using NextGen.Modules.Identity.Roles.Features.GetRoles;
using NextGen.Modules.Identity.Shared.Models;

namespace NextGen.Modules.Identity.Roles
{
    public class RolesMapping : Profile
    {
        public RolesMapping()
        {
            CreateMap<Role, GetRolesResponse>().ReverseMap();
        }
    }
}
