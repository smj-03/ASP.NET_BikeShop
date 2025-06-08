using BikeShop.Models;
using BikeShop.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BikeShop.Controllers;

[Authorize]
[Route("[controller]")]
public class UsersSearchController : Controller
{
    private readonly IUserSearchService _userSearchService;

    public UsersSearchController(IUserSearchService userSearchService)
    {
        _userSearchService = userSearchService;
    }

    // Wyświetla formularz wyszukiwania użytkowników
    [HttpGet("search")]
    public IActionResult Search()
    {
        return View(new UserSearchDto());
    }

    // Obsługuje wyszukiwanie (POST lub GET)
    [HttpPost("search")]
    public async Task<IActionResult> Search(UserSearchDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        var result = await _userSearchService.SearchUsersAsync(dto, User);
        return View("SearchResults", result);
    }
}