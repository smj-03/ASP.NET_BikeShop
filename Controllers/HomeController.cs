using System.Diagnostics;
using BikeShop.Models;
using BikeShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace BikeShop.Controllers;

public class HomeController : Controller
{
    private readonly IProductService _productService;
    
    public HomeController(IProductService productService)
    {
        _productService = productService;
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
    public async Task<IActionResult> Index([FromQuery] ProductFilterDto filter)
    {
        var products = await _productService.GetFilteredAsync(filter);
        var vm = new IndexViewModel
        {
            Filter = filter,
            Products = products
        };
        return View(vm);
    }
}