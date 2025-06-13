using NBomber.CSharp;
using NBomber.Contracts;
using NBomber.Contracts.Stats; // Added for ReportFormat
using Xunit;
using System.Net.Http; // Added for HttpClient
using System; // Added for AppContext, TimeSpan, Uri, Exception
using System.IO; // Added for Path

namespace BikeShop.Tests
{
    public class ProductControllerLoadTests
    {
        private readonly HttpClient _httpClient;

        public ProductControllerLoadTests()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5291") }; 
        }

        [Fact]
        public void Products_Index_Page_Load_Test()
        {
            var scenario = Scenario.Create("products_index_50_users_100_req_each", async context =>
                {
                    for (int i = 0; i < 100; i++) 
                    {
                        try
                        {
                            var response = await _httpClient.GetAsync("/Home");
                            response.EnsureSuccessStatusCode();
                        }
                        catch (Exception e)
                        {
                            return Response.Fail(message: $"Request {i + 1} for user {context.ScenarioInfo.ThreadNumber} failed: {e.Message}"); 
                        }
                    }
                    return Response.Ok(); 
                })
                .WithWarmUpDuration(TimeSpan.FromSeconds(10))
                .WithLoadSimulations(
                    Simulation.KeepConstant(
                        copies: 50, 
                        during: TimeSpan.FromMinutes(2))
                );

            var reportPath = Path.Combine(AppContext.BaseDirectory, "nbomber_reports");
            Console.WriteLine($"NBomber reports will be saved to: {reportPath}"); // Added for debugging

            NBomberRunner
                .RegisterScenarios(scenario)
                .WithReportFolder(reportPath) // Use absolute path
                .WithReportFormats(ReportFormat.Html, ReportFormat.Md) // Specifies report formats
                .Run();
        }
    }
}

