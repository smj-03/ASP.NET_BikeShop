using BikeShop.Models;
using BikeShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace BikeShop.Controllers
{
    public class BasketController : Controller
    {
        private readonly BasketService _basketService;
        private readonly IOrderService _orderService;

        public BasketController(BasketService basketService, IOrderService orderService)
        {
            _basketService = basketService;
            _orderService = orderService;
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

        [HttpPost]
        public async Task<IActionResult> Order()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var basket = _basketService.GetBasket();
            if (basket == null || !basket.Items.Any())
            {
                TempData["OrderError"] = "Koszyk jest pusty.";
                return RedirectToAction("Index");
            }

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var createOrderDto = new CreateOrderDto
            {
                CustomerId = userId,
                Products = basket.Items.Select(i => new OrderProductDto()
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity
                }).ToList()
            };

            await _orderService.CreateAsync(createOrderDto);
            _basketService.ClearBasket();
            TempData["OrderSuccess"] = "Zamówienie zostało złożone!";
            return RedirectToAction("Index", "Order");
        }
    }
}
