using BuildingBlocks.Abstractions.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NextGen.Modules.Identity;
using NextGen.Modules.Identity.ClaimGroups;
using NextGen.Modules.Identity.ClaimGroups.Features.GetClaimsByGroup;

public static class GetClaimsByGroupEndpoint
{
    public static IEndpointConventionBuilder MapGetClaimsByGroupEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet($"{ClaimGroupConfigs.ClaimGroupsPrefixUri}/{{groupId:guid}}/Claims", GetClaimsByGroup)
            .AllowAnonymous()
            .WithTags(ClaimGroupConfigs.Tag)
            .Produces<List<GetClaimsByGroupResponse>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("GetClaimsByGroup")
            .WithDisplayName("Get all Claims of a Claim Group")
            .WithApiVersionSet(ClaimGroupConfigs.VersionSet)
            .HasApiVersion(1.0);
    }

    private static async Task<IResult> GetClaimsByGroup(
        [FromRoute] Guid groupId,
        [FromServices] IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
        CancellationToken cancellationToken)
    {
        var query = new GetClaimsByGroupQuery(groupId);

        var result = await gatewayProcessor.ExecuteQuery(async processor =>
            await processor.SendAsync<List<GetClaimsByGroupResponse>>(query, cancellationToken));

        if (result == null || result.Count == 0)
            return Results.NotFound(new { message = "No claims found for this group." });

        return Results.Ok(result);
    }
}
