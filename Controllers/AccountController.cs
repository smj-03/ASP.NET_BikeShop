using BikeShop.Models;
using BikeShop.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
    private readonly IAccountService accountService;

    public AccountController(IAccountService accountService)
    {
        this.accountService = accountService;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await accountService.RegisterAsync(model);
            if (result.Succeeded)
                return RedirectToAction("Index", "Home");

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await accountService.LoginAsync(model);
            if (result.Succeeded)
                return RedirectToAction("ListUsers");

            ModelState.AddModelError(string.Empty, "Nieprawidłowe dane logowania.");
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult ListUsers()
    {
        var users = accountService.GetAllUsers();
        return View(users);
    }
}