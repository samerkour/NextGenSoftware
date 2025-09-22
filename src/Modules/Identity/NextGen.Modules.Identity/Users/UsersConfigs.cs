using Asp.Versioning.Builder;
using NextGen.Modules.Identity.Users.Features.GettingUerByEmail;
using NextGen.Modules.Identity.Users.Features.GettingUserById;
using NextGen.Modules.Identity.Users.Features.GettingUsers;
using NextGen.Modules.Identity.Users.Features.ProfilePicture;
using NextGen.Modules.Identity.Users.Features.RegisteringUser;
using NextGen.Modules.Identity.Users.Features.UpdatingUser;
using NextGen.Modules.Identity.Users.Features.UpdatingUserLockout;
using NextGen.Modules.Identity.Users.Features.DeleteUser;
using NextGen.Modules.Identity.Users.Features.ChangeUserPassword;

namespace NextGen.Modules.Identity.Users;

internal static class UsersConfigs
{
    public const string Tag = "Users";
    public const string UsersPrefixUri = $"{IdentityModuleConfiguration.IdentityModulePrefixUri}/users";
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
        endpoints.MapGetUserByEmailEndpoint();
        endpoints.MapUploadProfilePictureEndpoint();
        endpoints.MapRegisterNewUserEndpoint();
        endpoints.MapUpdateUserEndpoint();
        endpoints.MapUpdateUserStateEndpoint();
        endpoints.MapChangeUserPasswordEndpoint();
        endpoints.MapDeleteUserEndpoint();

        return endpoints;
    }
}
