using FluentValidation;
using BuildingBlocks.Abstractions.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NextGen.Modules.Identity.Claims.Features.UpdateClaim
{
    public static class UpdateClaimEndpoint
    {
        public static IEndpointConventionBuilder MapUpdateClaimEndpoint(this IEndpointRouteBuilder endpoints)
        {
            return endpoints.MapPut($"{ClaimConfigs.ClaimsPrefixUri}/{{id:guid}}", UpdateClaim)
                .AllowAnonymous()
                .WithTags(ClaimConfigs.Tag)
                .Produces<UpdateClaimResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest) // برای ولیدیشن
                .Produces(StatusCodes.Status404NotFound)
                .WithName("UpdateClaim")
                .WithDisplayName("Update an existing Claim")
                .WithApiVersionSet(ClaimConfigs.VersionSet)
                .HasApiVersion(1.0);
        }

        private static async Task<IResult> UpdateClaim(
            [FromRoute] Guid id,
            [FromBody] UpdateClaimRequest request,
            [FromServices] IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
            CancellationToken cancellationToken)
        {
            if (request == null)
                return Results.BadRequest("Request cannot be null.");

            var command = new UpdateClaimCommand(
                id,
                request.Type,
                request.Value,
                request.Name,
                request.Description,
                request.ClaimGroupId
            );

            // --- اجرای Validator ---
            var validator = new UpdateClaimValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                // Return structured 422 response for validation errors
                return Results.ValidationProblem(
                    validationResult.ToDictionary(),
                    statusCode: StatusCodes.Status422UnprocessableEntity
                );
            }

            // اجرای دستور Update از طریق Gateway
            var result = await gatewayProcessor.ExecuteCommand(async processor =>
                await processor.SendAsync<UpdateClaimResponse>(command, cancellationToken));

            if (result == null)
                return Results.NotFound($"Claim with Id {id} was not found.");

            return Results.Ok(result);
        }
    }
}
