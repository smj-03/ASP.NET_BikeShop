using Microsoft.AspNetCore.Identity;

namespace BikeShop.Services;

public class RoleInitializer : IRoleInitializer
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public RoleInitializer(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task EnsureRolesExistAsync()
    {
        string[] roles = { "Admin", "Employee", "Client" };

        foreach (var role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}