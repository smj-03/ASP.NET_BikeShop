using BikeShop.Mappers;
using BikeShop.Models;
using Microsoft.AspNetCore.Identity;
namespace BikeShop.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly SignInManager<ApplicationUser> signInManager;
    private readonly UserMapper userMapper;
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        UserMapper userMapper)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.userMapper = userMapper;
        _userManager = userManager;
    }

    public async Task<IdentityResult> AssignRoleAsync(string userId, string roleName)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null) return IdentityResult.Failed(new IdentityError { Description = "User not found" });

        // Tu możesz dodać walidację, że tylko Admin może wywołać tę metodę (np. w kontrolerze przez [Authorize(Roles = "Admin")])
        return await userManager.AddToRoleAsync(user, roleName);
    }
    public async Task<IdentityResult> RegisterAsync(RegisterViewModel model)
    {
        var user = userMapper.Map(model);
        user.UserName = model.Email; // Ustawienie UserName jako Email
        var result = await userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "Client");
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
    
    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        var users = userManager.Users.ToList();
        var userDtos = new List<UserDto>();

        foreach (var user in users)
        {
            var dto = userMapper.Map(user);
            var roles = await userManager.GetRolesAsync(user);
            dto.Role = roles.FirstOrDefault() ?? "No role";  // zakładam, że tylko jedna rola na użytkownika
            userDtos.Add(dto);
        }

        return userDtos;
    }
}