using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Abstractions.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NextGen.Modules.Identity.ClaimGroups.Features.GetClaimGroupById
{
    public static class GetClaimGroupByIdEndpoint
    {
        public static IEndpointConventionBuilder MapGetClaimGroupByIdEndpoint(this IEndpointRouteBuilder endpoints)
        {
            return endpoints.MapGet($"{ClaimGroupConfigs.ClaimGroupsPrefixUri}/{{id:guid}}", GetClaimGroupById)
                .AllowAnonymous()
                .WithTags(ClaimGroupConfigs.Tag)
                .Produces<GetClaimGroupByIdResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound)
                .WithName("GetClaimGroupById")
                .WithDisplayName("Get Claim Group by Id")
                .WithApiVersionSet(ClaimGroupConfigs.VersionSet)
                .HasApiVersion(1.0);
        }

        private static async Task<IResult> GetClaimGroupById(
            Guid id,
            [FromServices] IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
            CancellationToken cancellationToken)
        {
            var query = new GetClaimGroupByIdQuery(id);

            // اعتبارسنجی با FluentValidation
            var validator = new GetClaimGroupByIdValidator();
            var validationResult = await validator.ValidateAsync(query, cancellationToken);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .Select(e => new { e.PropertyName, e.ErrorMessage });
                return Results.BadRequest(errors);
            }

            // اجرای Query
            var result = await gatewayProcessor.ExecuteQuery(async processor =>
                await processor.SendAsync<GetClaimGroupByIdResponse?>(query, cancellationToken));

            // مدیریت رکورد موجود نبود
            if (result == null)
            {
                return Results.Json(
                    new { Message = $"ClaimGroup with Id '{id}' not found." },
                    statusCode: StatusCodes.Status404NotFound
                );
            }

            return Results.Ok(result);
        }
    }
}
