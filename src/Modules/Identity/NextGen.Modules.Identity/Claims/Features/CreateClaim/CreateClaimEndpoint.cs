using BuildingBlocks.Abstractions.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NextGen.Modules.Identity.Users;

namespace NextGen.Modules.Identity.Claims.Features.CreateClaim
{
    public static class CreateClaimEndpoint
    {
        public static IEndpointConventionBuilder MapCreateClaimEndpoint(this IEndpointRouteBuilder endpoints)
        {
           return endpoints.MapPost($"{ClaimConfigs.ClaimsPrefixUri}", CreateClaim)
                .AllowAnonymous()
                .WithTags(ClaimConfigs.Tag)
                .Produces<CreateClaimResponse>(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest)
                .WithName("CreateClaim")
                .WithDisplayName("Create a new Claim")
                .WithApiVersionSet(ClaimConfigs.VersionSet)
                .HasApiVersion(1.0);
        }

        private static async Task<IResult> CreateClaim(
            [FromBody] CreateClaimRequest request,
            [FromServices] IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
            CancellationToken cancellationToken)
        {
            if (request == null)
                return Results.BadRequest("Request cannot be null.");

            var command = new CreateClaimCommand(
                request.Type,
                request.Value,
                request.ClaimGroupId
            );

            var result = await gatewayProcessor.ExecuteCommand(async processor =>
                await processor.SendAsync<CreateClaimResponse>(command, cancellationToken));

            if (result == null)
                return Results.BadRequest("Failed to create claim.");

            return Results.Created($"{ClaimConfigs.ClaimsPrefixUri}/{result.Id}", result);
        }
    }
}
