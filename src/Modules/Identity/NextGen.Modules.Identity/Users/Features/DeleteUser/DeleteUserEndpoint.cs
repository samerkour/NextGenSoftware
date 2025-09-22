using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingBlocks.Abstractions.Web;

namespace NextGen.Modules.Identity.Users.Features.DeleteUser;

public static class DeleteUserEndpoint
{
    internal static IEndpointRouteBuilder MapDeleteUserEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete($"{UsersConfigs.UsersPrefixUri}/{{userId:guid}}", DeleteUser)
            .AllowAnonymous()
            .WithTags(UsersConfigs.Tag)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithName("DeleteUser")
            .WithDisplayName("Delete User.")
            .WithApiVersionSet(UsersConfigs.VersionSet)
            .HasApiVersion(1.0);

        return endpoints;
    }

    private static Task<IResult> DeleteUser(
        Guid userId,
        IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
        CancellationToken cancellationToken)
    {
        return gatewayProcessor.ExecuteCommand(async commandProcessor =>
        {
            var command = new DeleteUserCommand(userId);

            await commandProcessor.SendAsync(command, cancellationToken);

            return Results.NoContent();
        });
    }
}
