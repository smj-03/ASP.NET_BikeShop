namespace BikeShop.Models
{
    public class BasketItem
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }

    public class Basket
    {
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();
    }
}
