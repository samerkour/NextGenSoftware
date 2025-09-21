using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.Domain;
using BuildingBlocks.Core.Domain;
using BuildingBlocks.Core.Domain.ValueObjects;
using BuildingBlocks.Core.Exception;
using NextGen.Modules.Parties.Parties.ValueObjects;
using NextGen.Modules.Parties.RestockSubscriptions.Exceptions.Domain;
using NextGen.Modules.Parties.RestockSubscriptions.Features.CreatingRestockSubscription.Events.Domain;
using NextGen.Modules.Parties.RestockSubscriptions.Features.DeletingRestockSubscription;
using NextGen.Modules.Parties.RestockSubscriptions.Features.ProcessingRestockNotification;
using NextGen.Modules.Parties.RestockSubscriptions.ValueObjects;

namespace NextGen.Modules.Parties.RestockSubscriptions.Models.Write;

public class RestockSubscription : Aggregate<RestockSubscriptionId>, IHaveSoftDelete
{
    public PartyId PartyId { get; private set; } = default!;
    public Email Email { get; private set; } = default!;
    public ProductInformation ProductInformation { get; private set; } = default!;
    public bool Processed { get; private set; }
    public DateTime? ProcessedTime { get; private set; }

    public static RestockSubscription Create(
        RestockSubscriptionId id,
        PartyId partyId,
        ProductInformation productInformation,
        Email email)
    {
        Guard.Against.Null(id, new RestockSubscriptionDomainException("Id cannot be null"));
        Guard.Against.Null(partyId, new RestockSubscriptionDomainException("PartyId cannot be null"));
        Guard.Against.Null(
            productInformation,
            new RestockSubscriptionDomainException("ProductInformation cannot be null"));

        var restockSubscription = new RestockSubscription
        {
            Id = id, PartyId = partyId, ProductInformation = productInformation
        };

        restockSubscription.ChangeEmail(email);

        restockSubscription.AddDomainEvents(new RestockSubscriptionCreated(restockSubscription));

        return restockSubscription;
    }

    public void ChangeEmail(Email email)
    {
        Email = Guard.Against.Null(email, new RestockSubscriptionDomainException("Email can't be null."));
    }

    public void Delete()
    {
        AddDomainEvents(new RestockSubscriptionDeleted(this));
    }

    public void MarkAsProcessed(DateTime processedTime)
    {
        Processed = true;
        ProcessedTime = processedTime;

        AddDomainEvents(new RestockNotificationProcessed(this));
    }
}
