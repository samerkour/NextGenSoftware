using BuildingBlocks.Abstractions.Persistence;
using NextGen.Modules.Identity.Shared.Models;
using Microsoft.AspNetCore.Identity;

namespace NextGen.Modules.Identity.Identity.Data;

public class IdentityDataSeeder : IDataSeeder
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public IdentityDataSeeder(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task SeedAllAsync()
    {
        await SeedRoles();
        await SeedUsers();
    }

    private async Task SeedRoles()
    {
        if (!await _roleManager.RoleExistsAsync(ApplicationRole.Admin.Name))
            await _roleManager.CreateAsync(ApplicationRole.Admin);

        if (!await _roleManager.RoleExistsAsync(ApplicationRole.User.Name))
            await _roleManager.CreateAsync(ApplicationRole.User);
    }

    private async Task SeedUsers()
    {
        if (await _userManager.FindByEmailAsync("admin@test.com") == null)
        {
            var user = new ApplicationUser
            {
                UserName = "admin",
                FirstName = "Admin",
                LastName = "Test",
                Email = "admin@test.com",
            };

            var result = await _userManager.CreateAsync(user, "Admin@123456");

            if (result.Succeeded) await _userManager.AddToRoleAsync(user, ApplicationRole.Admin.Name);
        }

        if (await _userManager.FindByEmailAsync("admin2@test.com") == null)
        {
            var user = new ApplicationUser
            {
                UserName = "user",
                FirstName = "User",
                LastName = "Test",
                Email = "user@test.com"
            };

            var result = await _userManager.CreateAsync(user, "User@123456");

            if (result.Succeeded) await _userManager.AddToRoleAsync(user, ApplicationRole.User.Name);
        }
    }
}
