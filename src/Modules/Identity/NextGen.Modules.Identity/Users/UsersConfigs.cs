using Asp.Versioning.Builder;
using NextGen.Modules.Identity.Identity.Features.SendTOTP;
using NextGen.Modules.Identity.Identity.Features.VerifyTotp;
using NextGen.Modules.Identity.Users.Features.ChangeUserPassword;
using NextGen.Modules.Identity.Users.Features.DeleteUser;
using NextGen.Modules.Identity.Users.Features.GettingUserById;
using NextGen.Modules.Identity.Users.Features.GettingUsers;
using NextGen.Modules.Identity.Users.Features.ProfilePicture;
using NextGen.Modules.Identity.Users.Features.RegisteringUser;
using NextGen.Modules.Identity.Users.Features.UpdatingUser;
using NextGen.Modules.Identity.Users.Features.UpdatingUserLockout;
using NextGen.Modules.Identity.Users.Features.UserLockoutEnable;
using NextGen.Modules.Identity.Users.Features.UserTwoFactorEnable;

namespace NextGen.Modules.Identity.Users;

internal static class UsersConfigs
{
    public const string Tag = "users";
    public const string UsersPrefixUri = $"{IdentityModuleConfiguration.IdentityModulePrefixUri}/{Tag}";
    public static ApiVersionSet VersionSet { get; private set; } = default!;

    internal static IServiceCollection AddUsersServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services;
    }

    internal static IEndpointRouteBuilder MapUsersEndpoints(this IEndpointRouteBuilder endpoints)
    {
        VersionSet = endpoints.NewApiVersionSet(Tag).Build();

        endpoints.MapGetUsersEndpoint();
        endpoints.MapGetUserByIdEndpoint();
        endpoints.MapUploadProfilePictureEndpoint();
        endpoints.MapRegisterNewUserEndpoint();
        endpoints.MapSendTotpEndpoint();
        endpoints.MapUpdateUserEndpoint();
        endpoints.MapVerifyTotpEndpoint();
        endpoints.MapUpdateUserLockoutEndpoint();
        endpoints.MapUpdateUserLockoutEnabledEndpoint();
        endpoints.MapUpdateTwoFactorEndpoint();
        endpoints.MapChangeUserPasswordEndpoint();
        endpoints.MapDeleteUserEndpoint();

        return endpoints;
    }
}
