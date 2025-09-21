using Ardalis.GuardClauses;
using Asp.Versioning.Conventions;
using BuildingBlocks.Abstractions.Web;
using BuildingBlocks.Abstractions.Web.MinimalApi;

namespace NextGen.Modules.Parties.RestockSubscriptions.Features.CreatingRestockSubscription;

public class CreateRestockSubscriptionEndpoint : IMinimalEndpointDefinition
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost(RestockSubscriptionsConfigs.RestockSubscriptionsUrl, CreateRestockSubscription)
            .AllowAnonymous()
            .Produces<CreateRestockSubscriptionResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithTags(RestockSubscriptionsConfigs.Tag)
            .WithName("CreateRestockSubscription")
            .WithDisplayName("Register New RestockSubscription for Party.")
            .WithApiVersionSet(RestockSubscriptionsConfigs.VersionSet)
            .HasApiVersion(1.0)
            .HasApiVersion(2.0);

        return builder;
    }

    private static Task<IResult> CreateRestockSubscription(
        CreateRestockSubscriptionRequest request,
        IGatewayProcessor<PartiesModuleConfiguration> gatewayProcessor,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        return gatewayProcessor.ExecuteCommand(async commandProcessor =>
        {
            var command = new CreateRestockSubscription(request.PartyId, request.ProductId, request.Email);

            var result = await commandProcessor.SendAsync(command, cancellationToken);

            return Results.Created(
                $"{RestockSubscriptionsConfigs.RestockSubscriptionsUrl}/{result.RestockSubscription.Id}",
                result);
        });
    }
}
