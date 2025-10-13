using BuildingBlocks.Abstractions.Web;
using NextGen.Modules.Identity;
using NextGen.Modules.Identity.ClaimGroups;
using NextGen.Modules.Identity.ClaimGroups.Features.DeleteClaimGroup;

public static class DeleteClaimGroupEndpoint
{
    public static IEndpointRouteBuilder MapDeleteClaimGroupEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete($"{ClaimGroupConfigs.ClaimGroupsPrefixUri}/{{groupId:guid}}/{{isDeleted:bool}}", DeleteClaimGroup)
            .AllowAnonymous()
            .WithTags(ClaimGroupConfigs.Tag)
            .Produces<DeleteClaimGroupResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status422UnprocessableEntity)
            .WithName("DeleteClaimGroup")
            .WithDisplayName("Delete or Restore a Claim Group")
            .WithApiVersionSet(ClaimGroupConfigs.VersionSet)
            .HasApiVersion(1.0);

        return endpoints; 
    }

    private static async Task<IResult> DeleteClaimGroup(
        [FromRoute] Guid groupId,
        [FromRoute] bool isDeleted,
        [FromServices] IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
        CancellationToken cancellationToken)
    {
        var command = new DeleteClaimGroupCommand(groupId, isDeleted);

        var validator = new DeleteClaimGroupValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(
                validationResult.ToDictionary(),
                statusCode: StatusCodes.Status422UnprocessableEntity
            );
        }

        var result = await gatewayProcessor.ExecuteCommand(async processor =>
            await processor.SendAsync<DeleteClaimGroupResponse>(command, cancellationToken));

        return Results.Ok(result);
    }
}
