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
        this.signInManager = signInManager;
        this.userMapper = userMapper;
        this.userManager = userManager;
    }

    public async Task<IdentityResult> AssignRoleAsync(string userId, string roleName)
    {
        var user = await this.userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return IdentityResult.Failed(new IdentityError { Description = "User not found" });
        }

        // Tu możesz dodać walidację, że tylko Admin może wywołać tę metodę (np. w kontrolerze przez [Authorize(Roles = "Admin")])
        return await this.userManager.AddToRoleAsync(user, roleName);
    }

    public async Task<IdentityResult> RegisterAsync(RegisterViewModel model)
    {
        var user = this.userMapper.Map(model);
        user.UserName = model.Email; // Ustawienie UserName jako Email
        var result = await this.userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            await this.userManager.AddToRoleAsync(user, "Client");
            await this.signInManager.SignInAsync(user, isPersistent: false);
        }

        return result;
    }

    public async Task<SignInResult> LoginAsync(LoginViewModel model)
    {
        return await this.signInManager.PasswordSignInAsync(
            model.Email,
            model.Password,
            model.RememberMe,
            lockoutOnFailure: false);
    }

    public async Task LogoutAsync()
    {
        await this.signInManager.SignOutAsync();
    }

    public List<UserDto> GetAllUsers()
    {
        return this.userManager.Users.Select(u => this.userMapper.Map(u)).ToList();
    }

    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        var users = this.userManager.Users.ToList();
        var userDtos = new List<UserDto>();

        foreach (var user in users)
        {
            var dto = this.userMapper.Map(user);
            var roles = await this.userManager.GetRolesAsync(user);
            dto.Role = roles.FirstOrDefault() ?? "No role";  // zakładam, że tylko jedna rola na użytkownika
            userDtos.Add(dto);
        }

        return userDtos;
    }
    
    public IQueryable<UserDto> GetUsersQueryable()
    {
        return this.userManager.Users.Select(u => new UserDto
        {
            Email = u.Email,
            FirstName = u.FirstName,
            LastName = u.LastName,
            PhoneNumber = u.PhoneNumber,
        });
    }
}