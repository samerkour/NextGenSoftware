using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingBlocks.Abstractions.Web;

namespace NextGen.Modules.Identity.Users.Features.UserTwoFactorEnable;
public static class UpdateTwoFactorEndpoint
{
    internal static IEndpointRouteBuilder MapUpdateTwoFactorEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut($"{UsersConfigs.UsersPrefixUri}/{{userId:guid}}/twofactor/enable", UpdateTwoFactor)
            .AllowAnonymous()
            .WithTags(UsersConfigs.Tag)
            .Produces<bool>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status422UnprocessableEntity) // validation
            .Produces(StatusCodes.Status404NotFound) // not found
            .WithName("UpdateTwoFactor")
            .WithDisplayName("Enable or Disable User TwoFactor.")
            .WithApiVersionSet(UsersConfigs.VersionSet)
            .HasApiVersion(1.0);

        return endpoints;
    }

    private static Task<IResult> UpdateTwoFactor(
        Guid userId,
        UpdateTwoFactorRequest request,
        IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
        CancellationToken cancellationToken)
    {
        return gatewayProcessor.ExecuteCommand(async commandProcessor =>
        {
            var command = new UpdateTwoFactorCommand(userId, request.IsTwoFactorEnabled);

            // 1. Validate
            var validator = new UpdateTwoFactorValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                // Return structured 422 response for validation errors
                return Results.ValidationProblem(
                    validationResult.ToDictionary(),
                    statusCode: StatusCodes.Status422UnprocessableEntity
                );
            }

            // 2. Execute command
            var success = await commandProcessor.SendAsync<bool>(command, cancellationToken);

            return Results.Ok(success); // true on success
        });
    }
}
