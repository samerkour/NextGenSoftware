using BuildingBlocks.Abstractions.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NextGen.Modules.Identity;

namespace NextGen.Modules.Identity.Roles.Features.GetRoleById
{
    public static class GetRoleByIdEndpoint
    {
        public static IEndpointConventionBuilder MapGetRoleByIdEndpoint(this IEndpointRouteBuilder endpoints)
        {
            return endpoints.MapGet($"{RoleConfigs.RolesPrefixUri}/{{roleId:guid}}", GetRoleById)
                .AllowAnonymous()
                .WithTags(RoleConfigs.Tag)
                .Produces<GetRoleByIdResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithName("GetRoleById")
                .WithDisplayName("Get role by Id")
                .WithApiVersionSet(RoleConfigs.VersionSet)
                .HasApiVersion(1.0);
        }

        private static async Task<IResult> GetRoleById(
            [FromRoute] Guid roleId,
            [FromServices] IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
            CancellationToken cancellationToken)
        {
            try
            {
                var result = await gatewayProcessor.ExecuteQuery(async processor =>
                    await processor.SendAsync(new GetRoleByIdQuery(roleId), cancellationToken));

                return Results.Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return Results.NotFound(new { message = ex.Message });
            }
        }
    }
}
