using BikeShop.Models;
using QuestPDF;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace BikeShop.Documents;

public class MonthlySalesReportDocument
{
    private readonly List<SalesReportDto> data;

    public MonthlySalesReportDocument(List<SalesReportDto> data)
    {
        this.data = data;
    }

    public byte[] GeneratePdf()
    {
        QuestPDF.Settings.License = LicenseType.Community;

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(20);
                page.Content()
                    .Column(column =>
                    {
                        column.Item().Text("Raport sprzedaży").FontSize(20).Bold().Underline();

                        foreach (var order in this.data)
                        {
                            column.Item().Text($"Data zamówienia: {order.OrderDate:d}, Klient: {order.CustomerName}").Bold();

                            foreach (var item in order.Items)
                            {
                                column.Item().Text($" - ProduktId: {item.ProductId}, Ilość: {item.Quantity}, Cena: {item.Price:C}");
                            }

                            column.Item().Text($"Suma: {order.TotalPrice:C}");
                            column.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                        }
                    });
            });
        });

        using var ms = new MemoryStream();
        document.GeneratePdf(ms);
        return ms.ToArray();
    }
}
