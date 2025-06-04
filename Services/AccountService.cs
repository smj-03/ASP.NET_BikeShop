using BikeShop.Models;
using Microsoft.AspNetCore.Identity;

namespace BikeShop.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly SignInManager<ApplicationUser> signInManager;

    public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
    }

    public async Task<IdentityResult> RegisterAsync(RegisterViewModel model)
    {
        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            PhoneNumber = model.PhoneNumber
        };

        var result = await userManager.CreateAsync(user, model.Password);
        
        if (result.Succeeded)
        {
            await signInManager.SignInAsync(user, isPersistent: false);
        }

        return result;
    }

    public async Task<SignInResult> LoginAsync(LoginViewModel model)
    {
        return await signInManager.PasswordSignInAsync(
            model.Email,
            model.Password,
            model.RememberMe,
            lockoutOnFailure: false);
    }

    public async Task LogoutAsync()
    {
        await signInManager.SignOutAsync();
    }

    public List<ApplicationUser> GetAllUsers()
    {
        return userManager.Users.ToList();
    }
}