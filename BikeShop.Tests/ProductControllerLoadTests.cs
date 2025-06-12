using System;
using System.Net.Http;
using System.Threading.Tasks;
using NBomber.Contracts;
using NBomber.CSharp;
using Xunit;

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
            var scenario = Scenario.Create("products_index_load_test", async context =>
                {
                    try
                    {
                        var response = await _httpClient.GetAsync("/Products");
                        response.EnsureSuccessStatusCode();
                        return Response.Ok();
                    }
                    catch (Exception e)
                    {
                        return Response.Fail(e);
                    }
                })
                .WithWarmUpDuration(TimeSpan.FromSeconds(5))
                .WithLoadSimulations(
                    Simulation.Inject(rate: 10, 
                                      interval: TimeSpan.FromSeconds(1),
                                      during: TimeSpan.FromSeconds(30))
                );

            NBomberRunner
                .RegisterScenarios(scenario)
                .Run();
        }
    }
}

