using BuildingBlocks.Abstractions.Persistence;
using BuildingBlocks.Persistence.EfCore.SqlServer;
using NextGen.Modules.Identity.Shared.Data;
using NextGen.Modules.Identity.Shared.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace NextGen.Modules.Identity.Shared.Extensions.ServiceCollectionExtensions;

public static partial class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddCustomIdentity(
        this WebApplicationBuilder builder,
        IConfiguration configuration,
        string optionSection = nameof(IdentityOptions),
        Action<IdentityOptions>? configure = null)
    {
        AddCustomIdentity(builder.Services, configuration, optionSection, configure);

        return builder;
    }

    public static IServiceCollection AddCustomIdentity(
        this IServiceCollection services,
        IConfiguration configuration,
        string optionSection = nameof(IdentityOptions),
        Action<IdentityOptions>? configure = null)
    {
        // Problem with .net core identity - will override our default authentication scheme `JwtBearerDefaults.AuthenticationScheme` to unwanted `NextGen.Modules.Identity.Application` in `AddIdentity()` method .net identity
        // https://github.com/IdentityServer/IdentityServer4/issues/1525
        if (configuration.GetValue<bool>(
                $"{IdentityModuleConfiguration.ModuleName}:{nameof(SqlServerOptions)}:UseInMemory"))
        {
            services.AddDbContext<IdentityContext>(options =>
                options.UseInMemoryDatabase("NextGen.Modules.Identity"));

            services.AddScoped<IDbFacadeResolver>(provider => provider.GetService<IdentityContext>()!);
        }
        else
        {
            // SqlServer
            services.AddSqlServerDbContext<IdentityContext>(
                configuration,
                $"{IdentityModuleConfiguration.ModuleName}:{nameof(SqlServerOptions)}");
        }

        // https://github.com/IdentityServer/IdentityServer4/issues/1525
        // some dependencies will add here if not registered before
        services.AddIdentity<ApplicationUser, ApplicationRole>(
                options =>
                {
                    // Password settings.
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 3;
                    options.Password.RequiredUniqueChars = 1;

                    // Lockout settings.
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.AllowedForNewUsers = true;

                    // User settings.
                    options.User.RequireUniqueEmail = true;

                    options.SignIn.RequireConfirmedEmail = false;
                    options.SignIn.RequireConfirmedPhoneNumber = false;

                    if (configure is { })
                        configure.Invoke(options);
                })
            .AddEntityFrameworkStores<IdentityContext>()
            .AddDefaultTokenProviders();

        if (configuration.GetSection(optionSection) is not null)
            services.Configure<IdentityOptions>(configuration.GetSection(optionSection));

        return services;
    }
}
