using Asp.Versioning.Conventions;
using BuildingBlocks.Abstractions.Web;
using NextGen.Modules.Identity.Users.Features.RegisteringUser;

namespace NextGen.Modules.Identity.Users.Features.UpdatingUserLockout;

public static class UpdateUserLockoutEndpoint
{
    internal static IEndpointRouteBuilder MapUpdateUserStateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut($"{UsersConfigs.UsersPrefixUri}/{{userId:guid}}/lockout", UpdateUserState)
            .WithTags(UsersConfigs.Tag)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithName("UpdateUserLockout")
            .WithDisplayName("Update User Lockout.")
            .WithApiVersionSet(UsersConfigs.VersionSet)
            .HasApiVersion(1.0);

        return endpoints;
    }

    private static Task<IResult> UpdateUserState(
        Guid userId,
        UpdateUserLockoutRequest request,
        IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
        CancellationToken cancellationToken)
    {
        return gatewayProcessor.ExecuteCommand(async commandProcessor =>
        {
            var command = new UpdateUserLockoutCommand(userId, request.LockoutEnd);
            await commandProcessor.SendAsync(command, cancellationToken);
            return Results.NoContent();
        });
    }
}
