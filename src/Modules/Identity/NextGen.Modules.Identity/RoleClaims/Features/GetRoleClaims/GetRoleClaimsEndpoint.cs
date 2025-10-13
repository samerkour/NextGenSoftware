// GetRoleClaimsEndpoint.cs
using BuildingBlocks.Abstractions.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NextGen.Modules.Identity.Roles.Features.GetRoleClaims;

namespace NextGen.Modules.Identity.RoleClaims.Features.GetRoleClaims
{
    public static class GetRoleClaimsEndpoint
    {
        public static IEndpointConventionBuilder MapGetRoleClaimsEndpoint(this IEndpointRouteBuilder endpoints)
        {
            
            return endpoints.MapGet($"{RoleClaimConfigs.RoleClaimsPrefixUri}/roles/{{roleId}}/claims", GetRoleClaims)
                .AllowAnonymous()
                .WithTags(RoleClaimConfigs.Tag)
                .Produces<IEnumerable<RoleClaimResponse>>(StatusCodes.Status200OK)
                .WithName("GetRoleClaims")
                .WithDisplayName("Get Claims of a Role")
                .WithApiVersionSet(RoleClaimConfigs.VersionSet) 
                .HasApiVersion(1.0);
        }

        private static async Task<IResult> GetRoleClaims(
            [FromRoute] Guid roleId,
            [FromServices] IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
            CancellationToken cancellationToken)
        {
            var query = new GetRoleClaimsQuery(roleId);

            var result = await gatewayProcessor.ExecuteQuery(async processor =>
                await processor.SendAsync<IEnumerable<RoleClaimResponse>>(query, cancellationToken));

            return Results.Ok(result);
        }
    }
}
