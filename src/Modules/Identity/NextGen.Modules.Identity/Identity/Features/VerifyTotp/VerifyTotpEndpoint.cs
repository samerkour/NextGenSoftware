using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingBlocks.Abstractions.Web;
using NextGen.Modules.Identity.Users;

namespace NextGen.Modules.Identity.Identity.Features.VerifyTotp;
public static class VerifyTotpEndpoint
{
    internal static IEndpointRouteBuilder MapVerifyTotpEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost($"{IdentityConfigs.IdentityPrefixUri}/{{userId:guid}}/verify-totp", VerifyTotp)
            .AllowAnonymous()
            .WithTags(IdentityConfigs.Tag)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("VerifyTotp")
            .WithDisplayName("Verify TOTP Code")
            .WithApiVersionSet(IdentityConfigs.VersionSet)
            .HasApiVersion(1.0);

        return endpoints;
    }

    private static async Task<IResult> VerifyTotp(
      Guid userId,
      VerifyTotpRequest request,
      IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
      CancellationToken cancellationToken)
    {
        // 1. Map request -> command
        var command = new VerifyTotpCommand(userId, request.Code, request.DeliveryChannel);

        // 2. Validate command using VerifyTotpValidator
        var validator = new VerifyTotpValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        // 3. Process command if valid
        return await gatewayProcessor.ExecuteCommand(async commandProcessor =>
        {
            await commandProcessor.SendAsync(command, cancellationToken);
            return Results.Ok(new { Verified = true });
        });
    }
}
