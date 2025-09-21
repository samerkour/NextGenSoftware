using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asp.Versioning.Conventions;
using BuildingBlocks.Abstractions.Web;

namespace NextGen.Modules.Identity.Users.Features.UpdatingUser;

public static class UpdateUserEndpoint
{
    internal static IEndpointRouteBuilder MapUpdateUserEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut($"{UsersConfigs.UsersPrefixUri}/{{id:guid}}", UpdateUser)
            .WithTags(UsersConfigs.Tag)
            .Produces<UpdateUserResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("UpdateUser")
            .WithDisplayName("Update existing user.")
            .WithApiVersionSet(UsersConfigs.VersionSet)
            .HasApiVersion(1.0)
            .HasApiVersion(2.0);

        return endpoints;
    }

    private static Task<IResult> UpdateUser(
        Guid id,
        [FromBody] UpdateUserRequest request,
        IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
        CancellationToken cancellationToken)
    {
        return gatewayProcessor.ExecuteCommand(async commandProcessor =>
        {
            var command = new UpdateUserCommand(
                id,
                request.FirstName,
                request.LastName,
                request.UserName,
                request.Email,
                request.MiddleName,
                request.DateOfBirth,
                request.PlaceOfBirth,
                request.ProfileImagePath,
                request.Country,
                request.City,
                request.State,
                request.Address,
                request.PostalCode,
                request.Roles?.ToList()
            );

            var result = await commandProcessor.SendAsync(command, cancellationToken);
            return Results.Ok(result);
        });
    }
}
