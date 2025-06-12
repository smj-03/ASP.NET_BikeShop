using BikeShop.Models;
using BikeShop.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BikeShop.Controllers;

[Authorize(Roles = "Admin")]
public class ReportsController : Controller
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpPost]
    public async Task<IActionResult> MonthlySalesPdf(int year, int month)
    {
        // Ustalamy datę początku i końca miesiąca
        var startDate = new DateTime(year, month, 1);
        var endDate = startDate.AddMonths(1).AddTicks(-1); // ostatnia chwila miesiąca

        var reportData = await _reportService.GetSalesReport(startDate, endDate);
        var pdfBytes = _reportService.GenerateSalesReportPdf(reportData);

        var fileName = $"SalesReport_{year}_{month:D2}.pdf";
        return File(pdfBytes, "application/pdf", fileName);
    }
}
