using BuildingBlocks.Abstractions.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NextGen.Modules.Identity.ClaimGroups.Features.GetClaimGroups;

public static class GetClaimGroupsEndpoint
{
    public static IEndpointRouteBuilder MapGetClaimGroupsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(ClaimGroupConfigs.ClaimGroupsPrefixUri, GetClaimGroups)
            .AllowAnonymous()
            .WithTags(ClaimGroupConfigs.Tag)
            .Produces<GetClaimGroupsResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status422UnprocessableEntity)
            .WithName("GetClaimGroups")
            .WithDisplayName("Get all Claim Groups")
            .WithApiVersionSet(ClaimGroupConfigs.VersionSet)
            .HasApiVersion(1.0);

        return endpoints;
    }

    private static Task<IResult> GetClaimGroups(
        [FromQuery] int page,
        [FromQuery] int pageSize,
        [FromQuery] string[]? includes,
        [FromQuery] string[]? sorts,
        IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
        CancellationToken cancellationToken)
    {
        return gatewayProcessor.ExecuteQuery(async queryProcessor =>
        {
            var query = new GetClaimGroupsQuery
            {
                Includes = includes,
                Page = page,
                Sorts = sorts,
                PageSize = pageSize
            };

            var validator = new GetClaimGroupsValidator();
            var validationResult = await validator.ValidateAsync(query, cancellationToken);

            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(
                    validationResult.ToDictionary(),
                    statusCode: StatusCodes.Status422UnprocessableEntity
                );
            }

            var result = await queryProcessor.SendAsync(query, cancellationToken);
            return Results.Ok(result);
        });
    }
}
