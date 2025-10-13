using BuildingBlocks.Abstractions.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NextGen.Modules.Identity.RoleClaims.Features.DeleteRoleClaim
{
    public static class DeleteRoleClaimEndpoint
    {
        public static IEndpointConventionBuilder MapDeleteRoleClaimEndpoint(this IEndpointRouteBuilder endpoints)
        {
            return endpoints.MapDelete($"{RoleClaimConfigs.RoleClaimsPrefixUri}/roles/{{roleId:guid}}/claims/{{claimId:guid}}", DeleteRoleClaim)
                .AllowAnonymous()
                .WithTags(RoleClaimConfigs.Tag)
                .Produces<DeleteRoleClaimResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status422UnprocessableEntity)
                .WithName("DeleteRoleClaim")
                .WithDisplayName("Delete a Role Claim")
                .WithApiVersionSet(RoleClaimConfigs.VersionSet)
                .HasApiVersion(1.0);
        }

        private static async Task<IResult> DeleteRoleClaim(
            [FromRoute] Guid roleId,
            [FromRoute] Guid claimId,
            [FromServices] IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
            CancellationToken cancellationToken)
        {
            var command = new DeleteRoleClaimCommand(roleId, claimId);

            var validator = new DeleteRoleClaimValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(
                    validationResult.ToDictionary(),
                    statusCode: StatusCodes.Status422UnprocessableEntity
                );
            }

            try
            {
                var result = await gatewayProcessor.ExecuteCommand(async processor =>
                    await processor.SendAsync(command, cancellationToken));

                return Results.Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return Results.NotFound(new { error = ex.Message });
            }
        }
    }
}
