using BuildingBlocks.Abstractions.Persistence;
using Microsoft.AspNetCore.Identity;
using NextGen.Modules.Identity.Shared.Data;
using NextGen.Modules.Identity.Shared.Models;

namespace NextGen.Modules.Identity.Identity.Data;

public class IdentityDataSeeder : IDataSeeder
{
    private readonly RoleManager<Role> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IdentityContext _db;

    public IdentityDataSeeder(
        UserManager<ApplicationUser> userManager,
        RoleManager<Role> roleManager,
        IdentityContext db)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _db = db;
    }

    public async Task SeedAllAsync()
    {
        await SeedModulesAsync();
        await SeedRoleGroupsAsync();
        await SeedRolesAsync();
        await SeedClaimGroupsAsync();
        await SeedClaimsAsync();
        await SeedUsersAsync();
    }

    // ---------------------------
    // MODULES
    // ---------------------------
    private async Task SeedModulesAsync()
    {
        if (!_db.Modules.Any())
        {
            var modules = new[]
            {
                new NextGen.Modules.Identity.Shared.Models.ApplicationModule { Id = Guid.NewGuid(), Name = "Security", Description = "Handles authentication and authorization" },
                new NextGen.Modules.Identity.Shared.Models.ApplicationModule { Id = Guid.NewGuid(), Name = "Administration", Description = "System administration and settings" },
                new NextGen.Modules.Identity.Shared.Models.ApplicationModule { Id = Guid.NewGuid(), Name = "User", Description = "User self-service and profile management" }
            };

            _db.Modules.AddRange(modules);
            await _db.SaveChangesAsync();
        }
    }

    // ---------------------------
    // ROLE GROUPS
    // ---------------------------
    private async Task SeedRoleGroupsAsync()
    {
        if (!_db.RoleGroups.Any())
        {
            var securityModule = _db.Modules.First(m => m.Name == "Security");
            var adminModule = _db.Modules.First(m => m.Name == "Administration");
            var userModule = _db.Modules.First(m => m.Name == "User");

            var roleGroups = new[]
            {
                new RoleGroup { Id = Guid.NewGuid(), Name = "Security Group", ModuleId = securityModule.Id },
                new RoleGroup { Id = Guid.NewGuid(), Name = "Admin Group", ModuleId = adminModule.Id },
                new RoleGroup { Id = Guid.NewGuid(), Name = "User Group", ModuleId = userModule.Id }
            };

            _db.RoleGroups.AddRange(roleGroups);
            await _db.SaveChangesAsync();
        }
    }

    // ---------------------------
    // ROLES
    // ---------------------------
    private async Task SeedRolesAsync()
    {
        var securityGroup = _db.RoleGroups.First(rg => rg.Name == "Security Group");
        var adminGroup = _db.RoleGroups.First(rg => rg.Name == "Admin Group");
        var userGroup = _db.RoleGroups.First(rg => rg.Name == "User Group");

        if (!await _roleManager.RoleExistsAsync(Role.SecurityAdmin.Name))
        {
            var role = Role.SecurityAdmin;
            role.RoleGroupId = securityGroup.Id;
            await _roleManager.CreateAsync(role);
        }

        if (!await _roleManager.RoleExistsAsync(Role.Admin.Name))
        {
            var role = Role.Admin;
            role.RoleGroupId = adminGroup.Id;
            await _roleManager.CreateAsync(role);
        }

        if (!await _roleManager.RoleExistsAsync(Role.User.Name))
        {
            var role = Role.User;
            role.RoleGroupId = userGroup.Id;
            await _roleManager.CreateAsync(role);
        }
    }

    // ---------------------------
    // CLAIM GROUPS
    // ---------------------------
    private async Task SeedClaimGroupsAsync()
    {
        if (!_db.ClaimGroups.Any())
        {
            var groups = new[]
            {
                new ClaimGroup { Id = Guid.NewGuid(), Name = "Security Claims" },
                new ClaimGroup { Id = Guid.NewGuid(), Name = "Admin Claims" },
                new ClaimGroup { Id = Guid.NewGuid(), Name = "User Claims" }
            };

            _db.ClaimGroups.AddRange(groups);
            await _db.SaveChangesAsync();
        }
    }

    // ---------------------------
    // CLAIMS
    // ---------------------------
    private async Task SeedClaimsAsync()
    {
        if (!_db.Claims.Any())
        {
            var securityGroup = _db.ClaimGroups.First(cg => cg.Name == "Security Claims");
            var adminGroup = _db.ClaimGroups.First(cg => cg.Name == "Admin Claims");
            var userGroup = _db.ClaimGroups.First(cg => cg.Name == "User Claims");

            var claims = new List<ApplicationClaim>
            {
                // Security
                new ApplicationClaim { Id = Guid.NewGuid(), Type = "Permission", Value = "ManageUsers" },
                new ApplicationClaim { Id = Guid.NewGuid(), Type = "Permission", Value = "ManageRoles" },

                // Administration
                new ApplicationClaim { Id = Guid.NewGuid(), Type = "Permission", Value = "ViewDashboard" },
                new ApplicationClaim { Id = Guid.NewGuid(), Type = "Permission", Value = "ManageSettings" },

                // User
                new ApplicationClaim { Id = Guid.NewGuid(), Type = "Permission", Value = "ViewProfile" },
                new ApplicationClaim { Id = Guid.NewGuid(), Type = "Permission", Value = "EditProfile" }
            };

            _db.Claims.AddRange(claims);
            await _db.SaveChangesAsync();
        }
    }

    // ---------------------------
    // USERS
    // ---------------------------
    private async Task SeedUsersAsync()
    {
        if (await _userManager.FindByEmailAsync("securityAdmin@test.com") == null)
        {
            var user = new ApplicationUser
            {
                UserName = "securityAdmin",
                FirstName = "SecurityAdmin",
                LastName = "Test",
                Email = "securityAdmin@test.com"
            };

            var result = await _userManager.CreateAsync(user, "SecurityAdmin@123456");
            if (result.Succeeded)
                await _userManager.AddToRoleAsync(user, Role.SecurityAdmin.Name);
        }

        if (await _userManager.FindByEmailAsync("admin@test.com") == null)
        {
            var user = new ApplicationUser
            {
                UserName = "admin",
                FirstName = "Admin",
                LastName = "Test",
                Email = "admin@test.com"
            };

            var result = await _userManager.CreateAsync(user, "Admin@123456");
            if (result.Succeeded)
                await _userManager.AddToRoleAsync(user, Role.Admin.Name);
        }

        if (await _userManager.FindByEmailAsync("user@test.com") == null)
        {
            var user = new ApplicationUser
            {
                UserName = "user",
                FirstName = "User",
                LastName = "Test",
                Email = "user@test.com"
            };

            var result = await _userManager.CreateAsync(user, "User@123456");
            if (result.Succeeded)
                await _userManager.AddToRoleAsync(user, Role.User.Name);
        }
    }
}
