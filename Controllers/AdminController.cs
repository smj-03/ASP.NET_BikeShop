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
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly UserMapper _userMapper;

    public AdminController(UserManager<ApplicationUser> userManager, UserMapper userMapper)
    {
        _userManager = userManager;
        _userMapper = userMapper;
    }

    public async Task<IActionResult> ManageRoles()
    {
        var users = _userManager.Users.ToList();
        var userDtos = new List<UserDto>();

        foreach (var user in users)
        {
            var dto = _userMapper.Map(user);
            dto.Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault(); // dodanie roli
            userDtos.Add(dto);
        }

        return View(userDtos);
    }
    public async Task<IActionResult> ChangeRole(string email, string newRole)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return RedirectToAction("ManageRoles");

        var oldRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, oldRoles);

        var result = await _userManager.AddToRoleAsync(user, newRole);
        return RedirectToAction("ManageRoles");
    }
}