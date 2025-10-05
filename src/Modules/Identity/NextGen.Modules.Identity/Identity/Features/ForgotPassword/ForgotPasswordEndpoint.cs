using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Asp.Versioning.Conventions;
using BuildingBlocks.Abstractions.Web;
using global::NextGen.Modules.Identity.Users;

namespace NextGen.Modules.Identity.Identity.Features.ForgotPassword;

public static class ForgotPasswordEndpoint
{
    internal static IEndpointRouteBuilder MapForgotPasswordEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost($"{IdentityConfigs.IdentityPrefixUri}/forgot-password", ForgotPassword)
            .AllowAnonymous()
            .WithTags(IdentityConfigs.Tag)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("ForgotPassword")
            .WithDisplayName("Forgot Password.")
            .WithApiVersionSet(IdentityConfigs.VersionSet)
            .HasApiVersion(1.0);

        return endpoints;
    }

    private static Task<IResult> ForgotPassword(
        ForgotPasswordRequest request,
        IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
        CancellationToken cancellationToken)
    {
        return gatewayProcessor.ExecuteCommand(async commandProcessor =>
        {
            var command = new ForgotPasswordCommand(request.Email);

            await commandProcessor.SendAsync(command, cancellationToken);

            return Results.NoContent();
        });
    }
}
