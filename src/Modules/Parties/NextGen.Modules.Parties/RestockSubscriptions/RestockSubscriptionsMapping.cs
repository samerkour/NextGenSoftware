using AutoMapper;
using NextGen.Modules.Parties.RestockSubscriptions.Dtos;
using NextGen.Modules.Parties.RestockSubscriptions.Features;
using NextGen.Modules.Parties.RestockSubscriptions.Models.Read;
using NextGen.Modules.Parties.RestockSubscriptions.Models.Write;

namespace NextGen.Modules.Parties.RestockSubscriptions;

public class RestockSubscriptionsMapping : Profile
{
    public RestockSubscriptionsMapping()
    {
        CreateMap<RestockSubscription, RestockSubscriptionDto>()
            .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id.Value))
            .ForMember(x => x.Email, opt => opt.MapFrom(x => x.Email.Value))
            .ForMember(x => x.ProductName, opt => opt.MapFrom(x => x.ProductInformation.Name))
            .ForMember(x => x.ProductId, opt => opt.MapFrom(x => x.ProductInformation.Id.Value))
            .ForMember(x => x.PartyId, opt => opt.MapFrom(x => x.PartyId.Value));

        CreateMap<RestockSubscription, RestockSubscriptionReadModel>()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.RestockSubscriptionId, opt => opt.MapFrom(x => x.Id.Value))
            .ForMember(x => x.Email, opt => opt.MapFrom(x => x.Email.Value))
            .ForMember(x => x.ProductName, opt => opt.MapFrom(x => x.ProductInformation.Name))
            .ForMember(x => x.ProductId, opt => opt.MapFrom(x => x.ProductInformation.Id.Value))
            .ForMember(x => x.PartyId, opt => opt.MapFrom(x => x.PartyId.Value));

        CreateMap<RestockSubscriptionReadModel, RestockSubscriptionDto>()
            .ForMember(x => x.Id, opt => opt.MapFrom(x => x.RestockSubscriptionId))
            .ForMember(x => x.Email, opt => opt.MapFrom(x => x.Email))
            .ForMember(x => x.ProductName, opt => opt.MapFrom(x => x.ProductName))
            .ForMember(x => x.ProductId, opt => opt.MapFrom(x => x.ProductId))
            .ForMember(x => x.PartyId, opt => opt.MapFrom(x => x.PartyId));

        CreateMap<CreateMongoRestockSubscriptionReadModels, RestockSubscriptionReadModel>()
            .ForMember(x => x.RestockSubscriptionId, opt => opt.MapFrom(x => x.RestockSubscriptionId))
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.IsDeleted, opt => opt.MapFrom(x => x.IsDeleted));

        CreateMap<UpdateMongoRestockSubscriptionReadModel, RestockSubscriptionReadModel>()
            .ForMember(x => x.RestockSubscriptionId, opt => opt.MapFrom(x => x.RestockSubscription.Id.Value))
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.Created, opt => opt.MapFrom(x => x.RestockSubscription.Created))
            .ForMember(x => x.Email, opt => opt.MapFrom(x => x.RestockSubscription.Email.Value))
            .ForMember(x => x.Processed, opt => opt.MapFrom(x => x.RestockSubscription.Processed))
            .ForMember(x => x.PartyId, opt => opt.MapFrom(x => x.RestockSubscription.PartyId.Value))
            .ForMember(x => x.PartyName, opt => opt.Ignore())
            .ForMember(x => x.ProcessedTime, opt => opt.MapFrom(x => x.RestockSubscription.ProcessedTime))
            .ForMember(x => x.ProductId, opt => opt.MapFrom(x => x.RestockSubscription.ProductInformation.Id.Value))
            .ForMember(x => x.ProductName, opt => opt.MapFrom(x => x.RestockSubscription.ProductInformation.Name))
            .ForMember(x => x.IsDeleted, opt => opt.MapFrom(x => x.IsDeleted));
    }
}
