using BuildingBlocks.Abstractions.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NextGen.Modules.Identity;
using NextGen.Modules.Identity.ClaimGroups;
using NextGen.Modules.Identity.ClaimGroups.Features.CreateClaimGroup;

public static class CreateClaimGroupEndpoint
{
    public static IEndpointConventionBuilder MapCreateClaimGroupEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost($"{ClaimGroupConfigs.ClaimGroupsPrefixUri}", CreateClaimGroup)
            .AllowAnonymous()
            .WithTags(ClaimGroupConfigs.Tag)
            .Produces<CreateClaimGroupResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithName("CreateClaimGroup")
            .WithDisplayName("Create a new Claim Group")
            .WithApiVersionSet(ClaimGroupConfigs.VersionSet)
            .HasApiVersion(1.0);
    }

    private static async Task<IResult> CreateClaimGroup(
        [FromBody] CreateClaimGroupCommand command,
        [FromServices] IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
        CancellationToken cancellationToken)
    {
        // اعتبارسنجی با FluentValidation
        var validator = new CreateClaimGroupValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(e => new { e.PropertyName, e.ErrorMessage });
            return Results.BadRequest(errors);
        }

        // اگر اعتبارسنجی درست بود، ادامه پردازش
        var result = await gatewayProcessor.ExecuteCommand(async processor =>
            await processor.SendAsync<CreateClaimGroupResponse>(command, cancellationToken));

        return Results.Created($"{ClaimGroupConfigs.ClaimGroupsPrefixUri}/{result.Id}", result);
    }

}
