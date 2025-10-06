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
        endpoints.MapPut($"{UsersConfigs.UsersPrefixUri}/{{userId:guid}}", UpdateUser)
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

    private static async Task<IResult> UpdateUser(
        Guid userId,
        [FromBody] UpdateUserRequest request,
        IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
        CancellationToken cancellationToken)
    {
        // 1. Map request -> command
        var command = new UpdateUserCommand(
            userId,
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
            request.PostalCode
        );

        // 2. Validate command
        var validator = new UpdateUserValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            // Return structured 422 response for validation errors
            return Results.ValidationProblem(
                validationResult.ToDictionary(),
                statusCode: StatusCodes.Status422UnprocessableEntity
            );
        }

        // 3. Execute command if valid
        return await gatewayProcessor.ExecuteCommand(async commandProcessor =>
        {
            var result = await commandProcessor.SendAsync(command, cancellationToken);
            return Results.Ok(result);
        });
    }
}
