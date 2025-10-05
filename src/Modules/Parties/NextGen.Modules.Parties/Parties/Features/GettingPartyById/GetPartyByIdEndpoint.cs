using Ardalis.GuardClauses;
using Asp.Versioning.Conventions;
using BuildingBlocks.Abstractions.Web;
using BuildingBlocks.Abstractions.Web.MinimalApi;
using Swashbuckle.AspNetCore.Annotations;

namespace NextGen.Modules.Parties.Parties.Features.GettingPartyById;

public class GetPartyByIdEndpointEndpoint : IMinimalEndpointDefinition
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapGet(
                $"{PartiesConfigs.PartiesPrefixUri}/{{id}}",
                GetPartyById)
            .WithTags(PartiesConfigs.Tag)
            // .RequireAuthorization()
            .Produces<GetPartyByIdResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(PartiesConfigs.Tag)
            .WithMetadata(new SwaggerOperationAttribute("Getting a Party By Id", "Getting a Party By Id"))
            .WithName("GetPartyById")
            .WithDisplayName("Get Party By Id.")
            .WithApiVersionSet(PartiesConfigs.VersionSet)
            .HasApiVersion(1.0);

        return builder;
    }

    private static Task<IResult> GetPartyById(
        long id,
        IGatewayProcessor<PartiesModuleConfiguration> gatewayProcessor,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(id, nameof(id));

        return gatewayProcessor.ExecuteQuery(async queryProcessor =>
        {
            var result = await queryProcessor.SendAsync(new GetPartyById(id), cancellationToken);

            return Results.Ok(result);
        });
    }
}
