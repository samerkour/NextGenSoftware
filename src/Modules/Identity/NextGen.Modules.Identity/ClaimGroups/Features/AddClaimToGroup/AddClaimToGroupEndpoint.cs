using BuildingBlocks.Abstractions.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NextGen.Modules.Identity;
using NextGen.Modules.Identity.ClaimGroups;
using NextGen.Modules.Identity.ClaimGroups.Features.AddClaimToGroup;

public static class AddClaimToGroupEndpoint
{
    public static IEndpointConventionBuilder MapAddClaimToGroupEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost($"{ClaimGroupConfigs.ClaimGroupsPrefixUri}/{{groupId}}/Claims/{{claimId}}", AddClaimToGroup)
            .AllowAnonymous()
            .WithTags(ClaimGroupConfigs.Tag)
            .Produces<AddClaimToGroupResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("AddClaimToGroup")
            .WithDisplayName("Add Claim to a Claim Group")
            .WithApiVersionSet(ClaimGroupConfigs.VersionSet)
            .HasApiVersion(1.0);
    }

    private static async Task<IResult> AddClaimToGroup(
        [FromRoute] Guid groupId,
        [FromRoute] Guid claimId,
        [FromServices] IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
        CancellationToken cancellationToken)
    {
        var command = new AddClaimToGroupCommand(groupId, claimId);

        // اعتبارسنجی با FluentValidation
        var validator = new AddClaimToGroupValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(e => new { e.PropertyName, e.ErrorMessage });
            return Results.BadRequest(errors);
        }

        var result = await gatewayProcessor.ExecuteCommand(async processor =>
            await processor.SendAsync<AddClaimToGroupResponse>(command, cancellationToken));

        return Results.Ok(result);
    }
}
