using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingBlocks.Abstractions.Web;
using BuildingBlocks.Core.CQRS.Command;
using FluentValidation;
using NextGen.Modules.Identity.Users.Features.UpdatingUserLockout;

namespace NextGen.Modules.Identity.Users.Features.UserLockoutEnable;

public static class UpdateUserLockoutEnabledEndpoint
{
    internal static IEndpointRouteBuilder MapUpdateUserLockoutEnabledEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut($"{UsersConfigs.UsersPrefixUri}/{{userId:guid}}/lockout/enable", UpdateUserLockoutEnabled)
        .AllowAnonymous()
        .WithTags(UsersConfigs.Tag)
        .Produces<bool>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .WithName("UpdateUserLockoutEnabled")
        .WithDisplayName("Enable or Disable User Lockout.")
        .WithApiVersionSet(UsersConfigs.VersionSet)
        .HasApiVersion(1.0);

        return endpoints;
    }

    private static Task<IResult> UpdateUserLockoutEnabled(
    Guid userId,
    UpdateUserLockoutEnabledRequest request,
    IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
    CancellationToken cancellationToken)
    {
        return gatewayProcessor.ExecuteCommand(async commandProcessor =>
        {
            var command = new UpdateUserLockoutEnabledCommand(userId, request.IsLockoutEnabled);

            // Validate the command
            var validator = new UpdateUserLockoutEnabledValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                // Return structured 422 response for validation errors
                return Results.ValidationProblem(
                    validationResult.ToDictionary(),
                    statusCode: StatusCodes.Status422UnprocessableEntity
                );
            }

            // Execute the command and specify TResult explicitly
            var success = await commandProcessor.SendAsync(command, cancellationToken);

            return Results.Ok(success); // true if success, false if failure
        });
    }

}
