using BikeShop.Models;
using BikeShop.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BikeShop.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportsController : Controller
    {
        private readonly IReportService reportService;

        public ReportsController(IReportService reportService)
        {
            this.reportService = reportService;
        }

        // GET: Reports/MonthlySalesPdf
        [HttpGet]
        public IActionResult MonthlySalesPdf()
        {
            // Nie ma błędów na początku, więc pusta lista
            ViewData["AllErrors"] = new List<string>();
            return View();
        }

        // POST: Reports/MonthlySalesPdf
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MonthlySalesPdf(int year, int month)
        {
            if (year < 2000 || month < 1 || month > 12)
            {
                ModelState.AddModelError(string.Empty, "Podaj poprawny rok i miesiąc.");
            }

            if (!ModelState.IsValid)
            {
                // Przekazujemy listę błędów do widoku, aby uniknąć dynamicznych operacji w Razor
                var allErrors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                ViewData["AllErrors"] = allErrors;

                return View();
            }

            try
            {
                var startDate = new DateTime(year, month, 1);
                var endDate = startDate.AddMonths(1).AddTicks(-1); // ostatnia chwila miesiąca

                var reportData = await reportService.GetSalesReport(startDate, endDate);
                var pdfBytes = reportService.GenerateSalesReportPdf(reportData);

                var fileName = $"SalesReport_{year}_{month:D2}.pdf";
                return File(pdfBytes, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Wystąpił błąd podczas generowania raportu: " + ex.Message);

                var allErrors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                ViewData["AllErrors"] = allErrors;

                return View();
            }
        }
    }
}
