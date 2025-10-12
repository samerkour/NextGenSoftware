using BuildingBlocks.Abstractions.Web;
using Microsoft.AspNetCore.Mvc;
using NextGen.Modules.Identity;
using NextGen.Modules.Identity.ClaimGroups;
using NextGen.Modules.Identity.ClaimGroups.Features.UnassignClaimGroupFromRole;

public static class UnassignClaimGroupFromRoleEndpoint
{
    public static IEndpointConventionBuilder MapUnassignClaimGroupFromRoleEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete($"{ClaimGroupConfigs.ClaimGroupsPrefixUri}/{{groupId}}/Roles/{{roleId}}", UnassignClaimGroupFromRole)
            .AllowAnonymous()
            .WithTags(ClaimGroupConfigs.Tag)
            .Produces<UnassignClaimGroupFromRoleResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("UnassignClaimGroupFromRole")
            .WithDisplayName("Unassign a Claim Group from a Role")
            .WithApiVersionSet(ClaimGroupConfigs.VersionSet)
            .HasApiVersion(1.0);
    }

    private static async Task<IResult> UnassignClaimGroupFromRole(
        [FromRoute] Guid groupId,
        [FromRoute] Guid roleId,
        [FromServices] IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
        CancellationToken cancellationToken)
    {
        var command = new UnassignClaimGroupFromRoleCommand(groupId, roleId);

        var result = await gatewayProcessor.ExecuteCommand(async processor =>
            await processor.SendAsync<UnassignClaimGroupFromRoleResponse>(command, cancellationToken));

        return Results.Ok(result);
    }
}
