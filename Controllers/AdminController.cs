using BikeShop.Mappers;
using BikeShop.Models;
using BikeShop.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BikeShop.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly UserMapper userMapper;

    public AdminController(UserManager<ApplicationUser> userManager, UserMapper userMapper)
    {
        this.userManager = userManager;
        this.userMapper = userMapper;
    }

    public async Task<IActionResult> ManageRoles()
    {
        var users = this.userManager.Users.ToList();
        var userDtos = new List<UserDto>();

        foreach (var user in users)
        {
            var dto = this.userMapper.Map(user);
            dto.Role = (await this.userManager.GetRolesAsync(user)).FirstOrDefault(); // dodanie roli
            userDtos.Add(dto);
        }

        return this.View(userDtos);
    }

    public async Task<IActionResult> ChangeRole(string email, string newRole)
    {
        var user = await this.userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return this.RedirectToAction("ManageRoles");
        }

        var oldRoles = await this.userManager.GetRolesAsync(user);
        await this.userManager.RemoveFromRolesAsync(user, oldRoles);

        var result = await this.userManager.AddToRoleAsync(user, newRole);
        return this.RedirectToAction("ManageRoles");
    }
}