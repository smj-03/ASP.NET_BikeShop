using BikeShop.Mappers;
using BikeShop.Models;
using Microsoft.AspNetCore.Identity;
namespace BikeShop.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly SignInManager<ApplicationUser> signInManager;
    private readonly UserMapper userMapper;

    public AccountService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        UserMapper userMapper)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.userMapper = userMapper;
    }

    public async Task<IdentityResult> RegisterAsync(RegisterViewModel model)
    {
        var user = userMapper.Map(model);
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

    public List<UserDto> GetAllUsers()
    {
        return userManager.Users.Select(u => userMapper.Map(u)).ToList();
    }
}