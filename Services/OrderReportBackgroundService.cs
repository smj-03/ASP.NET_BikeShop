using System.Net;
using System.Net.Mail;

using QuestPDF.Fluent;
namespace BikeShop.Services;

public class OrderReportBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<OrderReportBackgroundService> _logger;
    private readonly string _adminEmail = "chwastekszymon@gmail.com"; // Change to real admin email
    private readonly string _pdfPath = "open_orders.pdf";

    public OrderReportBackgroundService(IServiceProvider serviceProvider, ILogger<OrderReportBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await GenerateAndSendReportAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in OrderReportBackgroundService");
            }
            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken); // Runs every 30 seconds
        }
    }

    private async Task GenerateAndSendReportAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<Data.AppDbContext>();
        var orders = db.Orders.ToList();

        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
        var pdfBytes = QuestPDF.Fluent.Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Header().Text("Open Orders Report").FontSize(20).Bold();
                page.Content().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(50);
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    });
                    table.Header(header =>
                    {
                        header.Cell().PaddingVertical(2).PaddingHorizontal(4).Text("ID").Bold();
                        header.Cell().PaddingVertical(2).PaddingHorizontal(4).Text("Customer").Bold();
                        header.Cell().PaddingVertical(2).PaddingHorizontal(4).Text("Date").Bold();
                        header.Cell().PaddingVertical(2).PaddingHorizontal(4).Text("Status").Bold();
                    });
                    foreach (var order in orders)
                    {
                        table.Cell().PaddingVertical(2).PaddingHorizontal(4).Text(order.Id.ToString());
                        table.Cell().PaddingVertical(2).PaddingHorizontal(4).Text(order.CustomerId?.ToString() ?? string.Empty);
                        table.Cell().PaddingVertical(2).PaddingHorizontal(4).Text(order.CreatedAt.ToString("yyyy-MM-dd"));
                        table.Cell().PaddingVertical(2).PaddingHorizontal(4).Text(order.Status.ToString() ?? string.Empty);
                    }
                });
            });
        }).GeneratePdf();

        await File.WriteAllBytesAsync(_pdfPath, pdfBytes);
    }
}
