using Asp.Versioning.Conventions;
using BuildingBlocks.Abstractions.Web;
using Microsoft.Extensions.Localization;

namespace NextGen.Modules.Identity.Identity.Features.Login;

public static class LoginEndpoint
{
    internal static IEndpointRouteBuilder MapLoginUserEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost($"{IdentityConfigs.IdentityPrefixUri}/login", LoginUser)
            .AllowAnonymous()
            .WithTags(IdentityConfigs.Tag)
            .Produces<LoginResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithDisplayName("Login User.")
            .WithApiVersionSet(IdentityConfigs.VersionSet)
            .HasApiVersion(1.0)
            .HasApiVersion(2.0);

        return endpoints;
    }

    private static async Task<IResult> LoginUser(
     LoginUserRequest request,
     IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
     IStringLocalizer<LoginValidator> validatorLocalizer,
     CancellationToken cancellationToken)
    {
        // 1. Map request → command
        var command = new LoginCommand(
            request.Captcha,
            request.UserNameOrEmail,
            request.Password,
            request.Remember
        );

        // 2. Validate command
        var validator = new LoginValidator(validatorLocalizer);
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        // 3. Execute if valid
        return await gatewayProcessor.ExecuteCommand(async commandProcessor =>
        {
            var result = await commandProcessor.SendAsync(command, cancellationToken);
            return Results.Ok(result);
        });
    }

}
