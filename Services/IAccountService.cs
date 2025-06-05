using BikeShop.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BikeShop.Services
{
    public interface IAccountService
    {
        Task<IdentityResult> RegisterAsync(RegisterViewModel model);
        Task<IdentityResult> AssignRoleAsync(string userId, string roleName);
        Task<SignInResult> LoginAsync(LoginViewModel model);
        Task LogoutAsync();
        List<UserDto> GetAllUsers();
        
        Task<List<UserDto>> GetAllUsersAsync();
    }
}