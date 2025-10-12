using BuildingBlocks.Abstractions.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NextGen.Modules.Identity;
using NextGen.Modules.Identity.ClaimGroups;
using NextGen.Modules.Identity.ClaimGroups.Features.GetClaimGroups;

public static class GetClaimGroupsEndpoint
{
    public static IEndpointConventionBuilder MapGetClaimGroupsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet($"{ClaimGroupConfigs.ClaimGroupsPrefixUri}", GetClaimGroups)
            .AllowAnonymous()
            .WithTags(ClaimGroupConfigs.Tag)
            .Produces<List<GetClaimGroupsResponse>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithName("GetClaimGroups")
            .WithDisplayName("Get list of Claim Groups")
            .WithApiVersionSet(ClaimGroupConfigs.VersionSet)
            .HasApiVersion(1.0);
    }

    private static async Task<IResult> GetClaimGroups(
        [FromQuery] string? searchTerm,
        [FromServices] IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
        CancellationToken cancellationToken)
    {
        var query = new GetClaimGroupsQuery(searchTerm);

        // اعتبارسنجی با FluentValidation
        var validator = new GetClaimGroupsValidator();
        var validationResult = await validator.ValidateAsync(query, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(e => new { e.PropertyName, e.ErrorMessage });
            return Results.BadRequest(errors);
        }

        // اگر معتبر بود، اجرای Query
        var result = await gatewayProcessor.ExecuteQuery(async processor =>
            await processor.SendAsync<List<GetClaimGroupsResponse>>(query, cancellationToken));

        return Results.Ok(result);
    }

}
