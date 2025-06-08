using BikeShop.Models;
using BikeShop.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BikeShop.Controllers;
 
[Authorize]
[ApiController]
[Route("api/users")]
public class UsersSearchController : Controller
{
    private readonly IUserSearchService _userSearchService;

    public UsersSearchController(IUserSearchService userSearchService)
    {
        _userSearchService = userSearchService;
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchUsers([FromQuery] UserSearchDto dto)
    {
        var result = await _userSearchService.SearchUsersAsync(dto, User);
        return Ok(result);
    }
}