using BuildingBlocks.Abstractions.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NextGen.Modules.Identity;
using NextGen.Modules.Identity.ClaimGroups;
using NextGen.Modules.Identity.ClaimGroups.Features.RemoveClaimFromGroup;

public static class RemoveClaimFromGroupEndpoint
{
    public static IEndpointConventionBuilder MapRemoveClaimFromGroupEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete($"{ClaimGroupConfigs.ClaimGroupsPrefixUri}/{{groupId:guid}}/Claims/{{claimId:guid}}", RemoveClaimFromGroup)
            .AllowAnonymous()
            .WithTags(ClaimGroupConfigs.Tag)
            .Produces<RemoveClaimFromGroupResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("RemoveClaimFromGroup")
            .WithDisplayName("Remove a Claim from a Claim Group")
            .WithApiVersionSet(ClaimGroupConfigs.VersionSet)
            .HasApiVersion(1.0);
    }

    private static async Task<IResult> RemoveClaimFromGroup(
       [FromRoute] Guid groupId,
       [FromRoute] Guid claimId,
       [FromServices] IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
       CancellationToken cancellationToken)
    {
        var command = new RemoveClaimFromGroupRequest(groupId, claimId);

        // اعتبارسنجی با FluentValidation
        var validator = new RemoveClaimFromGroupValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(e => new { e.PropertyName, e.ErrorMessage });
            return Results.BadRequest(errors); // 400
        }

        // اجرای Command
        var result = await gatewayProcessor.ExecuteCommand(processor =>
            processor.SendAsync<RemoveClaimFromGroupResponse>(command, cancellationToken));

        // بررسی وضعیت حذف
        return result.Removed
            ? Results.Ok(result) // 200
            : Results.NotFound(new { message = "Claim not found in this group." }); // 404
    }


}
