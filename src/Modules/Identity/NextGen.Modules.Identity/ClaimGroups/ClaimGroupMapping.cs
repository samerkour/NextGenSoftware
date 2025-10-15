using AutoMapper;
using NextGen.Modules.Identity.ClaimGroups.Features.CreateClaimGroup;
using NextGen.Modules.Identity.ClaimGroups.Features.GetClaimGroupById;
using NextGen.Modules.Identity.ClaimGroups.Features.GetClaimGroups;
using NextGen.Modules.Identity.ClaimGroups.Features.UpdateClaimGroup;
using NextGen.Modules.Identity.Shared.Models;

namespace NextGen.Modules.Identity.ClaimGroups
{
    public class ClaimGroupsMapping : Profile
    {
        public ClaimGroupsMapping()
        {
            // ---------------------------
            // GET All ClaimGroups
            // ---------------------------
            CreateMap<ClaimGroup, GetClaimGroupsResponse>();

            // ---------------------------
            // GET ClaimGroup by Id
            // ---------------------------
            CreateMap<ClaimGroup, GetClaimGroupByIdResponse>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name ?? string.Empty))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description ?? string.Empty))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.UpdatedOn, opt => opt.MapFrom(src => src.UpdatedOn));

            // ---------------------------
            // Create ClaimGroup
            // ---------------------------
            CreateMap<CreateClaimGroupCommand, ClaimGroup>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedOn, opt => opt.MapFrom(_ => DateTime.UtcNow));

            CreateMap<ClaimGroup, CreateClaimGroupResponse>();

            // ---------------------------
            // Update ClaimGroup
            // ---------------------------
            CreateMap<UpdateClaimGroupCommand, ClaimGroup>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Id از Route گرفته می‌شود
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.UpdatedOn, opt => opt.Ignore()); // فقط در Handler ست شود

            CreateMap<ClaimGroup, UpdateClaimGroupResponse>();
        }
    }
}
