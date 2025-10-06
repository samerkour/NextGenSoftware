using Asp.Versioning.Conventions;
using BuildingBlocks.Abstractions.Web;
using NextGen.Modules.Identity.Users.Features.RegisteringUser;

namespace NextGen.Modules.Identity.Users.Features.GettingUserById;

public static class GetUserByIdEndpoint
{
    internal static IEndpointRouteBuilder MapGetUserByIdEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet($"{UsersConfigs.UsersPrefixUri}/{{userId:guid}}", GetUserById)
            .AllowAnonymous()
            .WithTags(UsersConfigs.Tag)
            .Produces<RegisterUserResponse>(StatusCodes.Status200OK)
            .Produces<RegisterUserResponse>(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithName("GetUserById")
            .WithDisplayName("Get User by Id.")
            .WithApiVersionSet(UsersConfigs.VersionSet)
            .HasApiVersion(1.0);

        return endpoints;
    }

    private static Task<IResult> GetUserById(
     Guid userId,
     IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
     CancellationToken cancellationToken)
    {
        return gatewayProcessor.ExecuteQuery(async queryProcessor =>
        {
            // 1️⃣ Create the query
            var query = new GetUserByIdQuery(userId);

            // 2️⃣ Validate using FluentValidation
            var validator = new GetUserByIdValidator();
            var validationResult = await validator.ValidateAsync(query, cancellationToken);

            if (!validationResult.IsValid)
            {
                // Return structured 422 response for validation errors
                return Results.ValidationProblem(
                    validationResult.ToDictionary(),
                    statusCode: StatusCodes.Status422UnprocessableEntity
                );
            }

            // 3️⃣ Execute the query if valid
            var result = await queryProcessor.SendAsync(query, cancellationToken);

            // 4️⃣ Return the result
            return Results.Ok(result);
        });
    }
}
