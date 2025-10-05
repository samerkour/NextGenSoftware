using Asp.Versioning.Conventions;
using BuildingBlocks.Abstractions.Web;
using FluentValidation;
using NextGen.Modules.Identity.Users.Features.RegisteringUser;

namespace NextGen.Modules.Identity.Users.Features.UpdatingUserLockout;

public static class UpdateUserLockoutEndpoint
{
    internal static IEndpointRouteBuilder MapUpdateUserLockoutEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut($"{UsersConfigs.UsersPrefixUri}/{{userId:guid}}/lockout/lock", UpdateUserLockout)
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

    private static Task<IResult> UpdateUserLockout(
    Guid userId,
    UpdateUserLockoutRequest request,
    IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
    CancellationToken cancellationToken)
    {
        return gatewayProcessor.ExecuteCommand(async commandProcessor =>
        {
            var command = new UpdateUserLockoutCommand(userId, request.LockoutEnd);

            // 1. Validate
            var validator = new UpdateUserLockoutValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            // 2. Execute command if valid
            var success = await commandProcessor.SendAsync(command, cancellationToken);
            return Results.Ok(success); // true if success, false if failure
        });
    }

}
