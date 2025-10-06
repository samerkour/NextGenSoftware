using Asp.Versioning.Conventions;
using BuildingBlocks.Abstractions.Web;

namespace NextGen.Modules.Identity.Users.Features.RegisteringUser;

public static class RegisterUserEndpoint
{
    internal static IEndpointRouteBuilder MapRegisterNewUserEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost($"{UsersConfigs.UsersPrefixUri}", RegisterUser)
            .AllowAnonymous()
            .WithTags(UsersConfigs.Tag)
            .Produces<RegisterUserResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithName("RegisterUser")
            .WithDisplayName("Register New user.")
            .WithApiVersionSet(UsersConfigs.VersionSet)
            .HasApiVersion(1.0)
            .HasApiVersion(2.0);

        return endpoints;
    }

    private static async Task<IResult> RegisterUser(
     RegisterUserRequest request,
     IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
     CancellationToken cancellationToken)
    {
        // 1. Map request to command
        var command = new RegisterUserCommand(
            request.FirstName,
            request.LastName,
            request.UserName,
            request.Email,
            request.Password,
            request.ConfirmPassword,
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
        var validator = new RegisterUserValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            // Return structured 422 response for validation errors
            return Results.ValidationProblem(
                validationResult.ToDictionary(),
                statusCode: StatusCodes.Status422UnprocessableEntity
            );
        }

        // 3. Execute command
        var result = await gatewayProcessor.ExecuteCommand(async commandProcessor =>
        {
            return await commandProcessor.SendAsync(command, cancellationToken);
        });

        return Results.Created($"{UsersConfigs.UsersPrefixUri}/{result.UserIdentity?.Id}", result);
    }

}
