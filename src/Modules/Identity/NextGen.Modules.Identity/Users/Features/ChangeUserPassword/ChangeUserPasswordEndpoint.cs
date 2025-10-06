using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asp.Versioning.Conventions;
using BuildingBlocks.Abstractions.Web;

namespace NextGen.Modules.Identity.Users.Features.ChangeUserPassword;

public static class ChangeUserPasswordEndpoint
{
    internal static IEndpointRouteBuilder MapChangeUserPasswordEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut($"{UsersConfigs.UsersPrefixUri}/{{userId:guid}}/change-password", ChangePassword)
            .AllowAnonymous() // you may want to restrict this later
            .WithTags(UsersConfigs.Tag)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("ChangeUserPassword")
            .WithDisplayName("Change User Password.")
            .WithApiVersionSet(UsersConfigs.VersionSet)
            .HasApiVersion(1.0);

        return endpoints;
    }

    private static Task<IResult> ChangePassword(
     Guid userId,
     ChangeUserPasswordRequest request,
     IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
     CancellationToken cancellationToken)
    {
        return gatewayProcessor.ExecuteCommand(async commandProcessor =>
        {
            // 1. Create the command
            var command = new ChangeUserPasswordCommand(
                userId,
                request.CurrentPassword,
                request.NewPassword,
                request.ConfirmNewPassword
            );

            // 2. Validate the command
            var validator = new ChangeUserPasswordValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                // Return structured 422 response for validation errors
                return Results.ValidationProblem(
                    validationResult.ToDictionary(),
                    statusCode: StatusCodes.Status422UnprocessableEntity
                );
            }

            // 3. Execute the command if valid
            var success = await commandProcessor.SendAsync(command, cancellationToken);

            // 4. Return success status
            return Results.Ok(success); // true if password changed successfully
        });
    }

}
