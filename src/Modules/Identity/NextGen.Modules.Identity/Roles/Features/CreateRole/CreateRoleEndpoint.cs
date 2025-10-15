using BuildingBlocks.Abstractions.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NextGen.Modules.Identity;

namespace NextGen.Modules.Identity.Roles.Features.CreateRole
{
    public static class CreateRoleEndpoint
    {
        public static IEndpointConventionBuilder MapCreateRoleEndpoint(this IEndpointRouteBuilder endpoints)
        {
            return endpoints.MapPost($"{RoleConfigs.RolesPrefixUri}", CreateRole)
                .AllowAnonymous()
                .WithTags(RoleConfigs.Tag)
                .Produces<CreateRoleResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .WithName("CreateRole")
                .WithDisplayName("Create a new Role")
                .WithApiVersionSet(RoleConfigs.VersionSet)
                .HasApiVersion(1.0);
        }

        private static async Task<IResult> CreateRole(
            [FromBody] CreateRoleCommand command,
            [FromServices] IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
            CancellationToken cancellationToken)
        {
            var validator = new CreateRoleValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(
                    validationResult.ToDictionary(),
                    statusCode: StatusCodes.Status400BadRequest
                );
            }

            var result = await gatewayProcessor.ExecuteCommand(async processor =>
                await processor.SendAsync(command, cancellationToken));

            return Results.Ok(result);
        }
    }
}
