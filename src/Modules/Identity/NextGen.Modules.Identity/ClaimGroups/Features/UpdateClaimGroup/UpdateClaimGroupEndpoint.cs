using BuildingBlocks.Abstractions.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NextGen.Modules.Identity;
using NextGen.Modules.Identity.ClaimGroups;
using NextGen.Modules.Identity.ClaimGroups.Features.UpdateClaimGroup;

public static class UpdateClaimGroupEndpoint
{
    public static IEndpointConventionBuilder MapUpdateClaimGroupEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut($"{ClaimGroupConfigs.ClaimGroupsPrefixUri}/{{groupId:guid}}", UpdateClaimGroup)
            .AllowAnonymous()
            .WithTags(ClaimGroupConfigs.Tag)
            .Produces<UpdateClaimGroupResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("UpdateClaimGroup")
            .WithDisplayName("Update an existing Claim Group")
            .WithApiVersionSet(ClaimGroupConfigs.VersionSet)
            .HasApiVersion(1.0);
    }

    private static async Task<IResult> UpdateClaimGroup(
        [FromRoute] Guid groupId,
        [FromBody] UpdateClaimGroupRequest request,
        [FromServices] IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
        CancellationToken cancellationToken)
    {
        // ساخت Command
        var command = new UpdateClaimGroupCommand(groupId, request.Name, request.Description);

        // اجرای ولیدیشن با FluentValidation
        var validator = new UpdateClaimGroupValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(e => new { e.PropertyName, e.ErrorMessage });
            return Results.BadRequest(errors);
        }

        // اجرای Command
        var result = await gatewayProcessor.ExecuteCommand(async processor =>
            await processor.SendAsync<UpdateClaimGroupResponse>(command, cancellationToken));

        return Results.Ok(result);
    }

}
