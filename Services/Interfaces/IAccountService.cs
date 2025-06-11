namespace BikeShop.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BikeShop.Models;
    using Microsoft.AspNetCore.Identity;

    public interface IAccountService
    {
        Task<IdentityResult> RegisterAsync(RegisterViewModel model);

        Task<IdentityResult> AssignRoleAsync(string userId, string roleName);

        Task<SignInResult> LoginAsync(LoginViewModel model);

        Task LogoutAsync();

        List<UserDto> GetAllUsers();

        Task<List<UserDto>> GetAllUsersAsync();

        IQueryable<UserDto> GetUsersQueryable();
    }
}