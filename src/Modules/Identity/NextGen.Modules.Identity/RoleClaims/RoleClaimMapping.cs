using AutoMapper;
using NextGen.Modules.Identity.RoleClaims; // اگر DTO اینجا هست
using NextGen.Modules.Identity.Roles.Features.AddClaimToRole;
using NextGen.Modules.Identity.Roles.Features.GetRoleClaims;
using NextGen.Modules.Identity.Shared;
using NextGen.Modules.Identity.Shared.Models; // اگر RoleClaim اینجا هست

namespace NextGen.Modules.Identity.RoleClaims;

public class RoleClaimsMapping : Profile
{
    public RoleClaimsMapping()
    {
        // Entity → DTOs
        CreateMap<RoleClaim, RoleClaimResponse>().ReverseMap();

        // Command → Entity
        CreateMap<AddClaimToRoleCommand, RoleClaim>()
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
            .ForMember(dest => dest.ClaimId, opt => opt.MapFrom(src => src.ClaimId));
        // .ForAllOtherMembers(opt => opt.Ignore()); // اگر ارور داد، حذفش کن
    }
}
