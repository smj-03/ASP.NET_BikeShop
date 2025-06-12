using BikeShop.Data;
using BikeShop.Documents;
using BikeShop.Models;
using Microsoft.EntityFrameworkCore;

namespace BikeShop.Services;

public class ReportService : IReportService
{
    private readonly AppDbContext _context;

    public ReportService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<SalesReportDto>> GetSalesReport(DateTime startDate, DateTime endDate)
    {
        var orders = await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Items).ThenInclude(i => i.Product)
            .Where(o => o.CreatedAt >= startDate && o.CreatedAt <= endDate)
            .ToListAsync();

        return orders.Select(o => new SalesReportDto
        {
            OrderDate = o.CreatedAt,
            CustomerName = o.Customer.Email,
            Items = o.Items.Select(i => new OrderProductDto() {
                ProductId = i.Product.Id,
                Quantity = i.Quantity,
                Price = i.Product.Price
            }).ToList(),
            TotalPrice = o.Items.Sum(i => i.Quantity * i.Product.Price)
        }).ToList();
    }

    public byte[] GenerateSalesReportPdf(List<SalesReportDto> data)
    {
        var document = new MonthlySalesReportDocument(data);
        return document.GeneratePdf();
    }
}