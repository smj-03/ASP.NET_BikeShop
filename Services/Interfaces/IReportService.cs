using BikeShop.Models;

namespace BikeShop.Services;

public interface IReportService
{
    Task<List<SalesReportDto>> GetSalesReport(DateTime startDate, DateTime endDate);
    byte[] GenerateSalesReportPdf(List<SalesReportDto> data);
}