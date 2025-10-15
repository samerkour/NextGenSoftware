using BuildingBlocks.Abstractions.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NextGen.Modules.Identity.Roles.Features.DeleteRole
{
    public static class DeleteRoleEndpoint
    {
        public static IEndpointRouteBuilder MapDeleteRoleEndpoint(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapDelete($"{RoleConfigs.RolesPrefixUri}/{{id:guid}}/{{isDeleted:bool}}", DeleteRole)
                .AllowAnonymous()
                .WithTags(RoleConfigs.Tag)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status400BadRequest)
                .WithName("DeleteRole")
                .WithDisplayName("Delete an existing Role")
                .WithApiVersionSet(RoleConfigs.VersionSet)
                .HasApiVersion(1.0);

            return endpoints;
        }

        private static async Task<IResult> DeleteRole(
            [FromRoute] Guid id,
            [FromRoute] bool isDeleted,
            [FromServices] IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
            CancellationToken cancellationToken)
        {
            var command = new DeleteRoleCommand(id, isDeleted);

            // Validator
            var validator = new DeleteRoleValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );

                return Results.ValidationProblem(errors, statusCode: StatusCodes.Status422UnprocessableEntity);
            }

            var success = await gatewayProcessor.ExecuteCommand(async processor =>
                await processor.SendAsync(command, cancellationToken));

            return Results.Ok(success);
        }
    }
}
