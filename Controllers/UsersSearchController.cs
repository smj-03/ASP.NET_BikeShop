using BikeShop.Models;
using BikeShop.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BikeShop.Controllers;

[Authorize]
[Route("[controller]")]
public class UsersSearchController : Controller
{
    private readonly IUserSearchService userSearchService;

    public UsersSearchController(IUserSearchService userSearchService)
    {
        this.userSearchService = userSearchService;
    }

    // Wyświetla formularz wyszukiwania użytkowników
    [HttpGet("search")]
    public IActionResult Search()
    {
        return this.View(new UserSearchDto());
    }

    // Obsługuje wyszukiwanie (POST lub GET)
    [HttpPost("search")]
    public async Task<IActionResult> Search(UserSearchDto dto)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(dto);
        }

        var result = await this.userSearchService.SearchUsersAsync(dto, this.User);
        return this.View("SearchResults", result);
    }
}