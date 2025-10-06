// DeleteClaimEndpoint.cs
using BuildingBlocks.Abstractions.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NextGen.Modules.Identity.Claims.Features.DeleteClaim;

public static class DeleteClaimEndpoint
{
    public static IEndpointRouteBuilder MapDeleteClaimEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete($"{ClaimConfigs.ClaimsPrefixUri}/{{id:guid}}/{{isDeleted:bool}}", DeleteClaim)
            .AllowAnonymous()
            .WithTags(ClaimConfigs.Tag)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithName("DeleteClaim")
            .WithDisplayName("Delete an existing Claim")
            .WithApiVersionSet(ClaimConfigs.VersionSet)
            .HasApiVersion(1.0);

        return endpoints;
    }

    private static async Task<IResult> DeleteClaim(
        Guid id,
        [FromRoute] bool isDeleted,
        [FromServices] IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
        CancellationToken cancellationToken)
    {
        var command = new DeleteClaimCommand(id, isDeleted);

        // Validator
        var validator = new DeleteClaimValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            // Return structured 422 response for validation errors
            return Results.ValidationProblem(
                validationResult.ToDictionary(),
                statusCode: StatusCodes.Status422UnprocessableEntity
            );
        }

        var success = await gatewayProcessor.ExecuteCommand(async processor =>
            await processor.SendAsync(command, cancellationToken));

        return Results.Ok(success);
    }
}
