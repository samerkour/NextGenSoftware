using Microsoft.AspNetCore.Authorization;

namespace BuildingBlocks.Security.ApiKey.Authorization;

public class OnlyPartiesAuthorizationHandler : AuthorizationHandler<OnlyPartiesRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        OnlyPartiesRequirement requirement)
    {
        if (context.User.IsInRole(Roles.Party)) context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
