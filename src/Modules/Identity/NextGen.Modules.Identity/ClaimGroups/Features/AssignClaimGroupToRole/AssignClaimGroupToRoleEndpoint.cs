using BuildingBlocks.Abstractions.Web;
using NextGen.Modules.Identity;
using NextGen.Modules.Identity.ClaimGroups;
using NextGen.Modules.Identity.ClaimGroups.Features.AssignClaimGroupToRole;
using NextGen.Modules.Identity.Roles.Features.AssignClaimGroupToRole;

public static class ClaimGroupEndpoints
{
    public static IEndpointConventionBuilder MapAssignClaimGroupToRoleEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost($"{ClaimGroupConfigs.ClaimGroupsPrefixUri}/{{groupId}}/Roles/{{roleId}}", AssignClaimGroupToRole)
            .AllowAnonymous()
            .WithTags(ClaimGroupConfigs.Tag)
            .Produces<AssignClaimGroupToRoleResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithName("AssignClaimGroupToRole")
            .WithDisplayName("Assign ClaimGroup to Role")
            .WithApiVersionSet(ClaimGroupConfigs.VersionSet)
            .HasApiVersion(1.0);
    }

    private static async Task<IResult> AssignClaimGroupToRole(
        [FromRoute] Guid groupId,
        [FromRoute] Guid roleId,
        [FromServices] IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
        CancellationToken cancellationToken)
    {
        var command = new AssignClaimGroupToRoleCommand(groupId, roleId);

        var result = await gatewayProcessor.ExecuteCommand(async processor =>
            await processor.SendAsync<AssignClaimGroupToRoleResponse>(command, cancellationToken));

        return Results.Ok(result);
    }
}
