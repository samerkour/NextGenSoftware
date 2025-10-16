using BuildingBlocks.Abstractions.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NextGen.Modules.Identity;
using NextGen.Modules.Identity.ClaimGroups;
using NextGen.Modules.Identity.ClaimGroups.Features.UnassignClaimGroupFromRole;

public static class UnassignClaimGroupFromRoleEndpoint
{
    public static IEndpointConventionBuilder MapUnassignClaimGroupFromRoleEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete($"{ClaimGroupConfigs.ClaimGroupsPrefixUri}/{{groupId:guid}}/roles/{{roleId:guid}}", UnassignClaimGroupFromRole)
            .AllowAnonymous()
            .WithTags(ClaimGroupConfigs.Tag)
            .Produces<UnassignClaimGroupFromRoleResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status422UnprocessableEntity)
            .WithName("UnassignClaimGroupFromRole")
            .WithDisplayName("Soft delete (or restore) a Claim Group-Role relation")
            .WithApiVersionSet(ClaimGroupConfigs.VersionSet)
            .HasApiVersion(1.0);
    }

    private static async Task<IResult> UnassignClaimGroupFromRole(
        [FromRoute] Guid groupId,
        [FromRoute] Guid roleId,
        [FromQuery] bool isDeleted,
        [FromServices] IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
        CancellationToken cancellationToken)
    {
        var command = new UnassignClaimGroupFromRoleCommand(groupId, roleId, isDeleted);

        var validator = new UnassignClaimGroupFromRoleValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key ?? string.Empty,
                    g => g.Select(x => x.ErrorMessage).ToArray()
                );

            return Results.ValidationProblem(errors, statusCode: StatusCodes.Status422UnprocessableEntity);
        }

        var result = await gatewayProcessor.ExecuteCommand(async processor =>
            await processor.SendAsync(command, cancellationToken));

        if (!result.Success)
            return Results.NotFound(new { message = "Relation not found." });

        return Results.Ok(result);
    }
}
