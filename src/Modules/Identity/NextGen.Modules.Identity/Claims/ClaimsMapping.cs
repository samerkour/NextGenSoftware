// ClaimsMapping.cs
using AutoMapper;
using NextGen.Modules.Identity.Claims.Dtos;
using NextGen.Modules.Identity.Claims.Features.CreateClaim;
using NextGen.Modules.Identity.Claims.Features.DeleteClaim;
using NextGen.Modules.Identity.Claims.Features.GetClaimById;
using NextGen.Modules.Identity.Claims.Features.GetClaims;
using NextGen.Modules.Identity.Claims.Features.UpdateClaim;
using NextGen.Modules.Identity.Shared.Models;

namespace NextGen.Modules.Identity.Claims;

public class ClaimsMapping : Profile
{
    public ClaimsMapping()
    {
        // Entity → DTOs
        CreateMap<ApplicationClaim, ClaimDto>();

        CreateMap<ApplicationClaim, CreateClaimResponse>().ReverseMap();
        CreateMap<ApplicationClaim, UpdateClaimResponse>().ReverseMap();

        // Request → Entity
        CreateMap<CreateClaimRequest, ApplicationClaim>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedOn, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedOn, opt => opt.Ignore());

        CreateMap<UpdateClaimRequest, ApplicationClaim>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedOn, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.DeletedOn, opt => opt.Ignore());
    }

}
