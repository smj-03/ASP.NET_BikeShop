using BikeShop.Models;
using BikeShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace BikeShop.Controllers
{
    public class BasketController : Controller
    {
        private readonly BasketService _basketService;

        public BasketController(BasketService basketService)
        {
            _basketService = basketService;
        }

        public IActionResult Index()
        {
            var basket = _basketService.GetBasket();
            return View(basket);
        }

        [HttpPost]
        public IActionResult Add(int productId, string productName, decimal price, int quantity = 1)
        {
            var item = new BasketItem
            {
                ProductId = productId,
                ProductName = productName,
                Price = price,
                Quantity = quantity
            };
            _basketService.AddToBasket(item);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Remove(int productId)
        {
            _basketService.RemoveFromBasket(productId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Clear()
        {
            _basketService.ClearBasket();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Update(int productId, int quantity)
        {
            if (quantity < 1)
            {
                // Optionally, remove the item if quantity is set to less than 1
                _basketService.RemoveFromBasket(productId);
            }
            else
            {
                var basket = _basketService.GetBasket();
                var item = basket.Items.FirstOrDefault(i => i.ProductId == productId);
                if (item != null)
                {
                    item.Quantity = quantity;
                    _basketService.SaveBasket(basket);
                }
            }
            return RedirectToAction("Index");
        }
    }
}
