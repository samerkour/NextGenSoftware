using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingBlocks.Abstractions.Web;
using NextGen.Modules.Identity.Users;

namespace NextGen.Modules.Identity.Identity.Features.ResetPassword;
public static class ResetPasswordEndpoint
{
    internal static IEndpointRouteBuilder MapResetPasswordEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut($"{IdentityConfigs.IdentityPrefixUri}/reset-password/{{userId:guid}}", ResetPassword)
            .AllowAnonymous()
            .WithTags(IdentityConfigs.Tag)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("ResetPassword")
            .WithDisplayName("Reset User Password.")
            .WithApiVersionSet(IdentityConfigs.VersionSet)
            .HasApiVersion(1.0);

        return endpoints;
    }

    private static Task<IResult> ResetPassword(
        Guid userId,
        ResetPasswordRequest request,
        IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
        CancellationToken cancellationToken)
    {
        return gatewayProcessor.ExecuteCommand(async commandProcessor =>
        {
            var command = new ResetPasswordCommand(
                userId,
                request.ResetToken,
                request.NewPassword,
                request.ConfirmNewPassword
            );

            await commandProcessor.SendAsync(command, cancellationToken);

            return Results.NoContent();
        });
    }
}
