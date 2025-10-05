using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingBlocks.Abstractions.Web;
using NextGen.Modules.Identity.Identity.Features.Login;

namespace NextGen.Modules.Identity.Identity.Features.GenerateCaptcha
{
    public static class GenerateCaptchaEndpoint
    {
        internal static IEndpointRouteBuilder MapGenerateCaptchaEndpoint(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost($"{IdentityConfigs.IdentityPrefixUri}/GenerateCaptcha", (
                    object request,
                    IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
                    CancellationToken cancellationToken) =>
            {
                return gatewayProcessor.ExecuteCommand(async commandProcessor =>
                {
                    var command = new GenerateCaptchaCommand();

                    var result = await commandProcessor.SendAsync(command, cancellationToken);

                    return Results.Ok(result);
                });
            })
            .AllowAnonymous()
            .WithTags(IdentityConfigs.Tag)
            .Produces<LoginResponse>()
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithDisplayName("Generate Captcha.")
            .WithApiVersionSet(IdentityConfigs.VersionSet)
            .HasApiVersion(1.0)
            .HasApiVersion(2.0);

            return endpoints;
        }
    }
}
