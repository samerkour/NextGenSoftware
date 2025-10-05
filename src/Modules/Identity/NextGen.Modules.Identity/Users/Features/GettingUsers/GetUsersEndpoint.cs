using Asp.Versioning.Conventions;
using BuildingBlocks.Abstractions.CQRS;
using BuildingBlocks.Abstractions.Web;
using NextGen.Modules.Identity.Users.Features.RegisteringUser;

namespace NextGen.Modules.Identity.Users.Features.GettingUsers;

public static class GetUsersEndpoint
{
    internal static IEndpointRouteBuilder MapGetUsersEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(UsersConfigs.UsersPrefixUri, GetUsers)
            .AllowAnonymous()
            .WithTags(UsersConfigs.Tag)
            .Produces<GetUsersResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status400BadRequest)
            .WithName("GetUsers")
            .WithDisplayName("Get all users.")
            .WithApiVersionSet(UsersConfigs.VersionSet)
            .HasApiVersion(1.0);

        return endpoints;
    }

    private static Task<IResult> GetUsers(
      [FromQuery] int page,
      [FromQuery] int pageSize,
      [FromQuery] string[]? includes,
      [FromQuery] string[]? sorts,
      //[FromQuery] FilterModel[]? filters,
      IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
      CancellationToken cancellationToken)
    {
        var request = new GetUsersRequest
        {
            Page = page,
            PageSize = pageSize,
            Includes = includes,
            Sorts = sorts,
            //Filters = filters
        };

        return gatewayProcessor.ExecuteQuery(async queryProcessor =>
        {
            var result = await queryProcessor.SendAsync(
                new GetUsersQuery
                {
                    //Filters = request.Filters,
                    Includes = request.Includes,
                    Page = request.Page,
                    Sorts = request.Sorts,
                    PageSize = request.PageSize
                },
                cancellationToken);

            return Results.Ok(result);
        });
    }
}
