using BuildingBlocks.Core.CQRS.Query;
using NextGen.Modules.Customers.RestockSubscriptions.Dtos;

namespace NextGen.Modules.Customers.RestockSubscriptions.Features.GettingRestockSubscriptions;

public record GetRestockSubscriptionsResponse(ListResultModel<RestockSubscriptionDto> RestockSubscriptions);
