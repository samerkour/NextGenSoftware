using AutoMapper;
using NextGen.Modules.Identity.Roles.Dtos;
using NextGen.Modules.Identity.Roles.Features.CreateRole;
using NextGen.Modules.Identity.Roles.Features.GetRoles;
using NextGen.Modules.Identity.Roles.Features.UpdateRole;
using NextGen.Modules.Identity.Shared.Models;

namespace NextGen.Modules.Identity.Roles
{
    public class RolesMapping : Profile
    {
        public RolesMapping()
        {
            CreateMap<Role, GetRolesResponse>().ReverseMap();

            // Entity → DTOs
            CreateMap<Role, RoleDto>();

            CreateMap<Role, CreateRoleResponse>().ReverseMap();
            CreateMap<Role, UpdateRoleResponse>().ReverseMap();

            // Request → Entity
            CreateMap<CreateRoleRequest, Role>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedOn, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedOn, opt => opt.Ignore());

            CreateMap<UpdateRoleRequest, Role>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedOn, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.DeletedOn, opt => opt.Ignore());

        }
    }
}
