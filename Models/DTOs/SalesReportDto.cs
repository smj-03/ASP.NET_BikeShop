namespace BikeShop.Models;

public class SalesReportDto
{
    public DateTime OrderDate { get; set; }
    public string CustomerName { get; set; }
    public List<OrderProductDto> Items { get; set; }
    public decimal TotalPrice { get; set; }
}