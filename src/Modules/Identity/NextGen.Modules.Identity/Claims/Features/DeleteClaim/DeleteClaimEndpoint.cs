using BuildingBlocks.Abstractions.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NextGen.Modules.Identity.Claims.Features.DeleteClaim
{
    public static class DeleteClaimEndpoint
    {
        public static IEndpointConventionBuilder MapDeleteClaimEndpoint(this IEndpointRouteBuilder endpoints)
        {
            return endpoints.MapDelete($"{ClaimConfigs.ClaimsPrefixUri}/{{id:guid}}", DeleteClaim)
                .AllowAnonymous()
                .WithTags(ClaimConfigs.Tag)
                .Produces<DeleteClaimResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithName("DeleteClaim")
                .WithDisplayName("Delete an existing Claim")
                .WithApiVersionSet(ClaimConfigs.VersionSet)
                .HasApiVersion(1.0);
        }

        private static async Task<IResult> DeleteClaim(
            [FromRoute] Guid id,
            [FromServices] IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
            CancellationToken cancellationToken)
        {
            var command = new DeleteClaimCommand(id);

            // اجرای دستور حذف از طریق Gateway
            var result = await gatewayProcessor.ExecuteCommand(async processor =>
                await processor.SendAsync<DeleteClaimResponse>(command, cancellationToken));

            if (result == null || result.IsDeleted == false)
                return Results.NotFound($"Claim with Id {id} was not found.");

            return Results.Ok(result);
        }
    }
}
