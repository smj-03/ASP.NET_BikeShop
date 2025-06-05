using BikeShop.Models;
using BikeShop.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
    private readonly IAccountService accountService;
    private readonly UserManager<ApplicationUser> userManager;

    public AccountController(IAccountService accountService, UserManager<ApplicationUser> userManager)
    {
        this.accountService = accountService;
        this.userManager = userManager;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return this.View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (this.ModelState.IsValid)
        {
            var result = await this.accountService.RegisterAsync(model);
            if (result.Succeeded)
            {
                return this.RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                this.ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return this.View(model);
    }

    [HttpGet]
    public IActionResult Login()
    {
        return this.View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (this.ModelState.IsValid)
        {
            var result = await this.accountService.LoginAsync(model);
            if (result.Succeeded)
            {
                var user = await this.userManager.FindByEmailAsync(model.Email);
                if (user != null && await this.userManager.IsInRoleAsync(user, "Client"))
                {
                    return this.RedirectToAction("Index", "Home");
                }

                if (user != null && await this.userManager.IsInRoleAsync(user, "Admin"))
                {
                    return this.RedirectToAction("ManageRoles", "Admin");
                }

                return this.RedirectToAction("ListUsers");
            }

            this.ModelState.AddModelError(string.Empty, "Nieprawidłowe dane logowania.");
        }

        return this.View(model);
    }

    [HttpGet]
    public IActionResult ListUsers()
    {
        var users = this.accountService.GetAllUsers();
        return this.View(users);
    }
}