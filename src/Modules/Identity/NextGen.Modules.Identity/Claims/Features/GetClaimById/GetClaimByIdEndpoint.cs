using FluentValidation;
using BuildingBlocks.Abstractions.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NextGen.Modules.Identity.Claims.Features.GetClaimById
{
    public static class GetClaimByIdEndpoint
    {
        public static IEndpointConventionBuilder MapGetClaimByIdEndpoint(this IEndpointRouteBuilder endpoints)
        {
            return endpoints.MapGet($"{ClaimConfigs.ClaimsPrefixUri}/{{id:guid}}", GetClaimById)
                .AllowAnonymous()
                .WithTags(ClaimConfigs.Tag)
                .Produces<GetClaimByIdResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest) 
                .Produces(StatusCodes.Status404NotFound)
                .WithName("GetClaimById")
                .WithDisplayName("Get a Claim by Id")
                .WithApiVersionSet(ClaimConfigs.VersionSet)
                .HasApiVersion(1.0);
        }

        private static async Task<IResult> GetClaimById(
            [FromRoute] Guid id,
            [FromServices] IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
            CancellationToken cancellationToken)
        {
            var query = new GetClaimByIdQuery(id);

            //  Validator
            var validator = new GetClaimByIdValidator();
            var validationResult = await validator.ValidateAsync(query, cancellationToken);
            if (!validationResult.IsValid)
            {
                // Return structured 422 response for validation errors
                return Results.ValidationProblem(
                    validationResult.ToDictionary(),
                    statusCode: StatusCodes.Status422UnprocessableEntity
                );
            }

            // Command to Gateway

            var result = await gatewayProcessor.ExecuteQuery(async processor =>
                await processor.SendAsync<GetClaimByIdResponse>(query, cancellationToken));

            if (result is null)
                return Results.NotFound($"Claim with Id {id} was not found.");

            return Results.Ok(result);
        }
    }
}
