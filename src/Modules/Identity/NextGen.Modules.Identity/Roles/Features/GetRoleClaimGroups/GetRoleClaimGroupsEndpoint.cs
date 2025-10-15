using BuildingBlocks.Abstractions.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NextGen.Modules.Identity;

namespace NextGen.Modules.Identity.Roles.Features.GetRoleClaimGroups
{
    public static class GetRoleClaimGroupsEndpoint
    {
        public static IEndpointConventionBuilder MapGetRoleClaimGroupsEndpoint(this IEndpointRouteBuilder endpoints)
        {
            return endpoints.MapGet($"{RoleConfigs.RolesPrefixUri}/{{roleId:guid}}/ClaimGroups", GetRoleClaimGroups)
                .AllowAnonymous()
                .WithTags(RoleConfigs.Tag)
                .Produces<IEnumerable<GetRoleClaimGroupsResponse>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithName("GetRoleClaimGroups")
                .WithDisplayName("Get all ClaimGroups for a Role")
                .WithApiVersionSet(RoleConfigs.VersionSet)
                .HasApiVersion(1.0);
        }

        private static async Task<IResult> GetRoleClaimGroups(
            [FromRoute] Guid roleId,
            [FromServices] IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
            CancellationToken cancellationToken)
        {
            var result = await gatewayProcessor.ExecuteQuery(async processor =>
                await processor.SendAsync(new GetRoleClaimGroupsQuery(roleId), cancellationToken));

            if (!result.Any())
                return Results.NotFound(new { message = $"No ClaimGroups found for RoleId '{roleId}'." });

            return Results.Ok(result);
        }
    }
}
