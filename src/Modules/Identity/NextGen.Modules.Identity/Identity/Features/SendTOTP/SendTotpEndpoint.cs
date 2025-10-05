using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingBlocks.Abstractions.Web;
using NextGen.Modules.Identity.Users;

namespace NextGen.Modules.Identity.Identity.Features.SendTOTP;
public static class SendTotpEndpoint
{
    internal static IEndpointRouteBuilder MapSendTotpEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost($"{IdentityConfigs.IdentityPrefixUri}/{{userId:guid}}/send-totp", SendTotp)
            .AllowAnonymous() // can be anonymous for password reset, or restricted for MFA
            .WithTags(IdentityConfigs.Tag)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("SendTotp")
            .WithDisplayName("Send TOTP to User.")
            .WithApiVersionSet(IdentityConfigs.VersionSet)
            .HasApiVersion(1.0);

        return endpoints;
    }

    private static async Task<IResult> SendTotp(
     Guid userId,
     SendTotpRequest request,
     IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
     CancellationToken cancellationToken)
    {
        // 1. Map request → command
        var command = new SendTotpCommand(userId, request.DeliveryChannel);

        // 2. Validate
        var validator = new SendTotpValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        // 3. Execute if valid
        return await gatewayProcessor.ExecuteCommand(async commandProcessor =>
        {
            var totp = await commandProcessor.SendAsync(command, cancellationToken);
            return Results.Ok(new { Totp = totp });
        });
    }

}
