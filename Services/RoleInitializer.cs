using Microsoft.AspNetCore.Identity;

namespace BikeShop.Services;

public class RoleInitializer : IRoleInitializer
{
    private readonly RoleManager<IdentityRole> roleManager;

    public RoleInitializer(RoleManager<IdentityRole> roleManager)
    {
        this.roleManager = roleManager;
    }

    public async Task EnsureRolesExistAsync()
    {
        string[] roles = { "Admin", "Employee", "Client" };

        foreach (var role in roles)
        {
            if (!await this.roleManager.RoleExistsAsync(role))
            {
                await this.roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}