using BuildingBlocks.Abstractions.Web;
using Microsoft.AspNetCore.Http;

namespace NextGen.Modules.Identity.Claims.Features.GetClaims
{
    public static class GetClaimsEndpoint
    {
        public static IEndpointConventionBuilder MapGetClaimsEndpoint(this IEndpointRouteBuilder endpoints)
        {
            return endpoints.MapGet($"{ClaimConfigs.ClaimsPrefixUri}", GetClaims)

                .AllowAnonymous()
                .WithTags(ClaimConfigs.Tag)
                .Produces<List<Response>>(StatusCodes.Status200OK)
                .WithName("GetClaims")
                .WithDisplayName("Get all Claims")
                .WithApiVersionSet(ClaimConfigs.VersionSet)
                .HasApiVersion(1.0);
        }

        private static async Task<IResult> GetClaims(
            [FromServices] IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
            CancellationToken cancellationToken)
        {
            var query = new GetClaimsQuery();
            var result = await gatewayProcessor.ExecuteQuery(async processor =>
                await processor.SendAsync<List<Response>>(query, cancellationToken));

            return Results.Ok(result);
        }
    }
}
