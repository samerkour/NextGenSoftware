using Asp.Versioning.Conventions;
using BuildingBlocks.Abstractions.Web;

namespace NextGen.Modules.Identity.Claims.Features.GetClaims;

public static class GetClaimsEndpoint
{
    public static IEndpointRouteBuilder MapGetClaimsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(ClaimConfigs.ClaimsPrefixUri, GetClaims)
            .AllowAnonymous()
            .WithTags(ClaimConfigs.Tag)
            .Produces<GetClaimResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status422UnprocessableEntity)
            .WithName("GetClaims")
            .WithDisplayName("Get all Claims")
            .WithApiVersionSet(ClaimConfigs.VersionSet)
            .HasApiVersion(1.0);

        return endpoints;
    }

    private static Task<IResult> GetClaims(
    [FromQuery] int page,
    [FromQuery] int pageSize,
    [FromQuery] string[]? includes,
    [FromQuery] string[]? sorts,
    //[FromQuery] FilterModel[]? filters,
    IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
    CancellationToken cancellationToken)
    {
        return gatewayProcessor.ExecuteQuery(async queryProcessor =>
        {
            // 1️⃣ Create query
            var query = new GetClaimsQuery
            {
                //Filters = request.Filters,
                Includes = includes,
                Page = page,
                Sorts = sorts,
                PageSize = pageSize
            };

            // 2️⃣ Validate using FluentValidation
            var validator = new GetClaimsValidator();
            var validationResult = await validator.ValidateAsync(query, cancellationToken);

            if (!validationResult.IsValid)
            {
                // Return ValidationProblem (handled by middleware as 422)
                return Results.ValidationProblem(
                    validationResult.ToDictionary(),
                    statusCode: StatusCodes.Status422UnprocessableEntity
                );
            }

            // 3️⃣ Execute query if valid
            var result = await queryProcessor.SendAsync(query, cancellationToken);

            return Results.Ok(result);
        });
    }
}
