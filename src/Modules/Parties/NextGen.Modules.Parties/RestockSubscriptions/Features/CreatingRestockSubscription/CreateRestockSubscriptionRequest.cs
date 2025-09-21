namespace NextGen.Modules.Parties.RestockSubscriptions.Features.CreatingRestockSubscription;

public record CreateRestockSubscriptionRequest(long PartyId, long ProductId, string Email);
