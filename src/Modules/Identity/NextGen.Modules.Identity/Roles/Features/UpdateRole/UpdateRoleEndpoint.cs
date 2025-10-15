using BuildingBlocks.Abstractions.Web;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NextGen.Modules.Identity.Roles.Features.UpdateRole;

public static class UpdateRoleEndpoint
{
    public static IEndpointConventionBuilder MapUpdateRoleEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut($"{RoleConfigs.RolesPrefixUri}/{{id:guid}}", UpdateRole)
            .AllowAnonymous()
            .WithTags(RoleConfigs.Tag)
            .Produces<UpdateRoleResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("UpdateRole")
            .WithDisplayName("Update an existing Role")
            .WithApiVersionSet(RoleConfigs.VersionSet)
            .HasApiVersion(1.0);
    }

    private static async Task<IResult> UpdateRole(
        [FromRoute] Guid id,
        [FromBody] UpdateRoleRequest request,
        [FromServices] IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
        CancellationToken cancellationToken)
    {
        if (request is null)
            return Results.BadRequest("Request cannot be null.");

        var command = new UpdateRoleCommand(id, request.Name, request.Description);

        var validator = new UpdateRoleValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(
                validationResult.ToDictionary(),
                statusCode: StatusCodes.Status422UnprocessableEntity
            );
        }

        var result = await gatewayProcessor.ExecuteCommand(async processor =>
            await processor.SendAsync<UpdateRoleResponse>(command, cancellationToken));

        if (result is null)
            return Results.NotFound($"Role with Id {id} was not found.");

        return Results.Ok(result);
    }
}
