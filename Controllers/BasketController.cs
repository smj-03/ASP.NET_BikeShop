namespace BikeShop.Controllers
{
    using BikeShop.Models;
    using BikeShop.Services;
    using Microsoft.AspNetCore.Mvc;

    public class BasketController : Controller
    {
        private readonly BasketService basketService;
        private readonly IOrderService orderService;

        public BasketController(BasketService basketService, IOrderService orderService)
        {
            this.basketService = basketService;
            this.orderService = orderService;
        }

        public IActionResult Index()
        {
            var basket = this.basketService.GetBasket();
            return this.View(basket);
        }

        [HttpPost]
        public IActionResult Add(int productId, string productName, decimal price, int quantity = 1)
        {
            var item = new BasketItem
            {
                ProductId = productId,
                ProductName = productName,
                Price = price,
                Quantity = quantity,
            };
            this.basketService.AddToBasket(item);
            return this.RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Remove(int productId)
        {
            this.basketService.RemoveFromBasket(productId);
            return this.RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Clear()
        {
            this.basketService.ClearBasket();
            return this.RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Update(int productId, int quantity)
        {
            if (quantity < 1)
            {
                // Optionally, remove the item if quantity is set to less than 1
                this.basketService.RemoveFromBasket(productId);
            }
            else
            {
                var basket = this.basketService.GetBasket();
                var item = basket.Items.FirstOrDefault(i => i.ProductId == productId);
                if (item != null)
                {
                    item.Quantity = quantity;
                    this.basketService.SaveBasket(basket);
                }
            }

            return this.RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Order()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction("Login", "Account");
            }

            var basket = this.basketService.GetBasket();
            if (basket == null || !basket.Items.Any())
            {
                this.TempData["OrderError"] = "Koszyk jest pusty.";
                return this.RedirectToAction("Index");
            }

            var userId = this.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var createOrderDto = new CreateOrderDto
            {
                CustomerId = userId,
                Products = basket.Items.Select(i => new OrderProductDto()
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity
                }).ToList(),
            };

            await this.orderService.CreateAsync(createOrderDto);
            this.basketService.ClearBasket();
            this.TempData["OrderSuccess"] = "Zamówienie zostało złożone!";
            return this.RedirectToAction("Index", "Order");
        }
    }
}
