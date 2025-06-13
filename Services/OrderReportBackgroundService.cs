using System.Net;
using System.Net.Mail;
using QuestPDF.Fluent;

namespace BikeShop.Services;

public class OrderReportBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    private readonly ILogger<OrderReportBackgroundService> _logger;

// (Line removed)
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
                        table.Cell().PaddingVertical(2).PaddingHorizontal(4)
                            .Text(order.CustomerId?.ToString() ?? string.Empty);
                        table.Cell().PaddingVertical(2).PaddingHorizontal(4)
                            .Text(order.CreatedAt.ToString("yyyy-MM-dd"));
                        table.Cell().PaddingVertical(2).PaddingHorizontal(4)
                            .Text(order.Status.ToString() ?? string.Empty);
                    }
                });
            });
        }).GeneratePdf();

        await File.WriteAllBytesAsync(_pdfPath, pdfBytes);
        
        await SendEmailWithAttachmentAsync("chwastekszymon@gmail.com", "Order", "Attached is the report of open orders.", _pdfPath);
    }

    private async Task SendEmailWithAttachmentAsync(string to, string subject, string body, string attachmentPath)
    {
        var message = new MailMessage("important@interia.com", to, subject, body);
        message.Attachments.Add(new Attachment(attachmentPath));

        using (var client = new SmtpClient("poczta.interia.pl")
               {
                   Port = 587,
                   Credentials = new NetworkCredential("important@interia.com", "canon1"),
                   EnableSsl = true
               })
        {
            await client.SendMailAsync(message);
            Console.WriteLine("Wysłano e-mail do: " + to);
        }
    }
}