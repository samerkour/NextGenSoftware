using AutoMapper;
using NextGen.Modules.Identity.Claims.Features.CreateClaim;
using NextGen.Modules.Identity.Claims.Features.DeleteClaim;
using NextGen.Modules.Identity.Shared.Models;

namespace NextGen.Modules.Identity.Claims;

public class ClaimsMapping : Profile
{
    public ClaimsMapping()
    {
        // Entity -> Response
        CreateMap<ApplicationClaim, CreateClaimResponse>();

        // Request -> Entity
        CreateMap<CreateClaimRequest, ApplicationClaim>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

        CreateMap<ApplicationClaim, ApplicationClaim>();

        CreateMap<DeleteClaimCommand, DeleteClaimResponse>()
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => true));
    }
}
