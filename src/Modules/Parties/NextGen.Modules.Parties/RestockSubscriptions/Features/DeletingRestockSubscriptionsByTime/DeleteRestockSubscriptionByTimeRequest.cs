namespace NextGen.Modules.Parties.RestockSubscriptions.Features.DeletingRestockSubscriptionsByTime;

public record DeleteRestockSubscriptionByTimeRequest(DateTime? From = null, DateTime? To = null);
