using System.Security.Claims;
using BikeShop.Models;

namespace BikeShop.Services;

public interface IUserSearchService
{
    Task<List<UserDto>> SearchUsersAsync(UserSearchDto searchDto, ClaimsPrincipal requester);
}