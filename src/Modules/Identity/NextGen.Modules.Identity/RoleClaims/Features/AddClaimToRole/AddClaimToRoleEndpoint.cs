// AddClaimToRoleEndpoint.cs
using BuildingBlocks.Abstractions.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NextGen.Modules.Identity.Roles.Features.AddClaimToRole;

namespace NextGen.Modules.Identity.RoleClaims.Features.AddClaimToRole
{
    public static class AddClaimToRoleEndpoint
    {
        public static IEndpointConventionBuilder MapAddClaimToRoleEndpoint(this IEndpointRouteBuilder endpoints)
        {
            return endpoints.MapPost($"{RoleClaimConfigs.RoleClaimsPrefixUri}/roles/{{roleId}}/claims", AddClaimToRole)
                .AllowAnonymous()
                .WithTags(RoleClaimConfigs.Tag)
                .Produces(StatusCodes.Status200OK)
                .WithName("AddClaimToRole")
                .WithDisplayName("Add Claim to a Role")
                .WithApiVersionSet(RoleClaimConfigs.VersionSet)
                .HasApiVersion(1.0);
        }

        private static async Task<IResult> AddClaimToRole(
            [FromRoute] Guid roleId,
            [FromBody] Guid claimId,
            [FromServices] IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
            CancellationToken cancellationToken)
        {
            var command = new AddClaimToRoleCommand(roleId, claimId);

            // اجرای ولیدیشن
            var validator = new AddClaimToRoleValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
            {
                // جمع‌آوری پیام خطاها
                var errors = validationResult.Errors.Select(e => e.ErrorMessage);
                return Results.BadRequest(new { Errors = errors });
            }

            // اجرای Command
            await gatewayProcessor.ExecuteCommand(async processor =>
                await processor.SendAsync(command, cancellationToken));

            return Results.Ok();
        }

    }
}
