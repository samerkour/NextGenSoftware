using Ardalis.GuardClauses;
using Asp.Versioning.Conventions;
using BuildingBlocks.Abstractions.Web;
using BuildingBlocks.Abstractions.Web.MinimalApi;
using Swashbuckle.AspNetCore.Annotations;

namespace NextGen.Modules.Parties.Parties.Features.CreatingParty;

public class CreatePartyEndpoint : IMinimalEndpointDefinition
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost(PartiesConfigs.PartiesPrefixUri, CreateParty)
            .AllowAnonymous()
            .Produces<CreatePartyResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags(PartiesConfigs.Tag)
            .WithMetadata(new SwaggerOperationAttribute("Creating a Party", "Creating a Party"))
            .WithName("CreateParty")
            .WithDisplayName("Register New Party.")
            .WithApiVersionSet(PartiesConfigs.VersionSet)
            .HasApiVersion(1.0)
            .HasApiVersion(2.0);

        return builder;
    }

    private static Task<IResult> CreateParty(
        CreatePartyRequest request,
        IGatewayProcessor<PartiesModuleConfiguration> gatewayProcessor,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        return gatewayProcessor.ExecuteCommand(async commandProcessor =>
        {
            var command = new CreateParty(request.Email);

            var result = await commandProcessor.SendAsync(command, cancellationToken);

            return Results.Created($"{PartiesConfigs.PartiesPrefixUri}/{result.PartyId}", result);
        });
    }
}
