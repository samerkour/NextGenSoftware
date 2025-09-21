using BuildingBlocks.Core.CQRS.Query;
using NextGen.Modules.Parties.RestockSubscriptions.Dtos;

namespace NextGen.Modules.Parties.RestockSubscriptions.Features.GettingRestockSubscriptions;

public record GetRestockSubscriptionsResponse(ListResultModel<RestockSubscriptionDto> RestockSubscriptions);
