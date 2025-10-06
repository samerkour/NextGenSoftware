using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingBlocks.Abstractions.Web;
using FluentValidation;
using NextGen.Modules.Identity.Identity.Features.SendTOTP;

namespace NextGen.Modules.Identity.Users.Features.DeleteUser;

public static class DeleteUserEndpoint
{
    internal static IEndpointRouteBuilder MapDeleteUserEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete($"{UsersConfigs.UsersPrefixUri}/{{userId:guid}}/{{isDeleted:bool}}", DeleteUser)
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
         [FromRoute]bool isDeleted,
         IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
         CancellationToken cancellationToken)
    {
        return gatewayProcessor.ExecuteCommand(async commandProcessor =>
        {
            var command = new DeleteUserCommand(userId, isDeleted);

            // 2. Validate
            var validator = new DeleteUserValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                // Return structured 422 response for validation errors
                return Results.ValidationProblem(
                    validationResult.ToDictionary(),
                    statusCode: StatusCodes.Status422UnprocessableEntity
                );
            }

            // 3. Execute if valid
            var success = await commandProcessor.SendAsync(command, cancellationToken);
            return Results.Ok(success);
        });
    }
}
