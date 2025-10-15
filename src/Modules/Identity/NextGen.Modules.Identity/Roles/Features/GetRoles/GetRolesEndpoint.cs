using BuildingBlocks.Abstractions.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NextGen.Modules.Identity.Roles.Dtos;

namespace NextGen.Modules.Identity.Roles.Features.GetRoles;

public static class GetRolesEndpoint
{
    public static IEndpointRouteBuilder MapGetRolesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(RoleConfigs.RolesPrefixUri, GetRoles)
            .AllowAnonymous()
            .WithTags(RoleConfigs.Tag)
            .Produces<GetRolesResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status422UnprocessableEntity)
            .WithName("GetRoles")
            .WithDisplayName("Get all Roles")
            .WithApiVersionSet(RoleConfigs.VersionSet)
            .HasApiVersion(1.0);

        return endpoints;
    }

    private static Task<IResult> GetRoles(
        [FromQuery] int page,
        [FromQuery] int pageSize,
        [FromQuery] string[]? includes,
        [FromQuery] string[]? sorts,
        IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
        CancellationToken cancellationToken)
    {
        return gatewayProcessor.ExecuteQuery(async queryProcessor =>
        {
            var query = new GetRolesQuery
            {
                Includes = includes,
                Page = page,
                Sorts = sorts,
                PageSize = pageSize
            };

            var validator = new GetRolesValidator();
            var validationResult = await validator.ValidateAsync(query, cancellationToken);

            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(
                    validationResult.ToDictionary(),
                    statusCode: StatusCodes.Status422UnprocessableEntity
                );
            }

            var result = await queryProcessor.SendAsync(query, cancellationToken);
            return Results.Ok(result);
        });
    }
}
