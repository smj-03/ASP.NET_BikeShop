namespace BikeShop.Services
{
    using BikeShop.Models;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;

    public class BasketService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private const string BasketSessionKey = "Basket";

        public BasketService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public Basket GetBasket()
        {
            var session = this.httpContextAccessor.HttpContext.Session;
            var basketJson = session.GetString(BasketSessionKey);
            if (string.IsNullOrEmpty(basketJson))
            {
                return new Basket();
            }

            return JsonConvert.DeserializeObject<Basket>(basketJson) ?? new Basket();
        }

        public void SaveBasket(Basket basket)
        {
            var session = this.httpContextAccessor.HttpContext.Session;
            var basketJson = JsonConvert.SerializeObject(basket);
            session.SetString(BasketSessionKey, basketJson);
        }

        public void AddToBasket(BasketItem item)
        {
            var basket = this.GetBasket();
            var existing = basket.Items.FirstOrDefault(i => i.ProductId == item.ProductId);
            if (existing != null)
            {
                existing.Quantity += item.Quantity;
            }
            else
            {
                basket.Items.Add(item);
            }

            this.SaveBasket(basket);
        }

        public void RemoveFromBasket(int productId)
        {
            var basket = this.GetBasket();
            var item = basket.Items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                basket.Items.Remove(item);
                this.SaveBasket(basket);
            }
        }

        public void ClearBasket()
        {
            this.SaveBasket(new Basket());
        }
    }
}
