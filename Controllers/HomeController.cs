using System.Diagnostics;
using BikeShop.Models;
using BikeShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace BikeShop.Controllers;

public class HomeController : Controller
{
    private readonly IProductService productService;

    public HomeController(IProductService productService)
    {
        this.productService = productService;
    }

    public IActionResult Privacy()
    {
        return this.View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
    }

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] ProductFilterDto filter, int page = 1, int pageSize = 9)
    {
        var allProducts = await this.productService.GetFilteredAsync(filter);
        var totalCount = allProducts.Count();
        var products = allProducts.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        var vm = new IndexViewModel
        {
            Filter = filter,
            Products = products,
            CurrentPage = page,
            PageSize = pageSize,
            TotalPages = totalPages,
            TotalCount = totalCount,
        };
        return this.View(vm);
    }
}
