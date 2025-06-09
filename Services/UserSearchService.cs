using System.Security.Claims;
using BikeShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BikeShop.Services;

public class UserSearchService : IUserSearchService
{
    private readonly UserManager<ApplicationUser> userManager;

    public UserSearchService(UserManager<ApplicationUser> userManager)
    {
        this.userManager = userManager;
    }

    public async Task<List<UserDto>> SearchUsersAsync(UserSearchDto searchDto, ClaimsPrincipal requester)
    {
        var requesterUser = await this.userManager.GetUserAsync(requester);
        var requesterRoles = await this.userManager.GetRolesAsync(requesterUser);

        bool isAdmin = requesterRoles.Contains("Admin");
        bool isEmployee = requesterRoles.Contains("Employee");

        if (!isAdmin && !isEmployee)
        {
            throw new UnauthorizedAccessException("Brak uprawnień.");
        }

        var allowedRoles = isAdmin
            ? new[] { "Client", "Employee" }
            : new[] { "Client" };

        var users = await this.userManager.Users.ToListAsync();
        var result = new List<UserDto>();

        foreach (var user in users)
        {
            var roles = await this.userManager.GetRolesAsync(user);

            if (!roles.Any(r => allowedRoles.Contains(r)))
            {
                continue;
            }

            if (searchDto.Role != null && !roles.Contains(searchDto.Role))
            {
                continue;
            }

            if (!string.IsNullOrWhiteSpace(searchDto.Query))
            {
                string q = searchDto.Query.Trim().ToLowerInvariant();

                if (!(user.FirstName?.ToLower().Contains(q) == true ||
                      user.LastName?.ToLower().Contains(q) == true ||
                      user.Email?.ToLower().Contains(q) == true ||
                      user.PhoneNumber?.ToLower().Contains(q) == true))
                {
                    continue;
                }
            }

            result.Add(new UserDto
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Role = roles.FirstOrDefault() ?? string.Empty,
            });
        }

        return result;
    }
}