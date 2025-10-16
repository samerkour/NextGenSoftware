using BuildingBlocks.Abstractions.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NextGen.Modules.Identity.ClaimGroups.Features.RemoveClaimFromGroup
{
    public static class RemoveClaimFromGroupEndpoint
    {
        public static IEndpointRouteBuilder MapRemoveClaimFromGroupEndpoint(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapDelete($"{ClaimGroupConfigs.ClaimGroupsPrefixUri}/{{groupId:guid}}/Claims/{{claimId:guid}}/{{isDeleted:bool}}", DeleteClaimFromGroup)
                .AllowAnonymous()
                .WithTags(ClaimGroupConfigs.Tag)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithName("RemoveClaimFromGroup")
                .WithDisplayName("Soft Delete a Claim from a Claim Group")
                .WithApiVersionSet(ClaimGroupConfigs.VersionSet)
                .HasApiVersion(1.0);

            return endpoints;
        }

        private static async Task<IResult> DeleteClaimFromGroup(
            [FromRoute] Guid groupId,
            [FromRoute] Guid claimId,
            [FromRoute] bool isDeleted,
            [FromServices] IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
            CancellationToken cancellationToken)
        {
            var command = new RemoveClaimFromGroupCommand(groupId, claimId, isDeleted);

            var success = await gatewayProcessor.ExecuteCommand(processor =>
                processor.SendAsync(command, cancellationToken));

            return success
                ? Results.Ok(new { GroupId = groupId, ClaimId = claimId, IsDeleted = isDeleted })
                : Results.NotFound(new { message = "Claim not found in this group." });
        }
    }
}
