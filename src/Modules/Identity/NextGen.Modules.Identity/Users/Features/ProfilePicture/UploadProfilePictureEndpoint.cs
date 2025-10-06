using System.Security.Claims;
using BuildingBlocks.Abstractions.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using NextGen.Modules.Identity.Shared.Models;
using NextGen.Modules.Identity.Users;

namespace NextGen.Modules.Identity.Users.Features.ProfilePicture;

public static class UploadProfilePictureEndpoint
{
    internal static IEndpointRouteBuilder MapUploadProfilePictureEndpoint(this IEndpointRouteBuilder endpoints) 
    {
        endpoints.MapPost($"{UsersConfigs.UsersPrefixUri}/upload-profile-picture", UploadProfilePicture)
            .AllowAnonymous() // adjust if authorization is required
            .WithTags(UsersConfigs.Tag)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("UploadProfilePicture")
            .WithDisplayName("Upload user profile picture.")
            .WithApiVersionSet(UsersConfigs.VersionSet)
            .DisableAntiforgery()
            .HasApiVersion(1.0);

        return endpoints;
    }

    private static Task<IResult> UploadProfilePicture(
    IFormFile file,
    IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
    CancellationToken cancellationToken)
    {
        return gatewayProcessor.ExecuteCommand(async commandProcessor =>
        {
            var command = new UploadProfilePictureCommand(file);

            // 1. Validate
            var validator = new UploadProfilePictureCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                // Return structured 422 response for validation errors
                return Results.ValidationProblem(
                    validationResult.ToDictionary(),
                    statusCode: StatusCodes.Status422UnprocessableEntity
                );
            }

            // Send the command to the handler
            var result = await commandProcessor.SendAsync(command, cancellationToken);

            return Results.Ok(new { imagePath = result.ImagePath });
        });
    }

}
