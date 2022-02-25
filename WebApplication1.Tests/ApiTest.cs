using Microsoft.AspNetCore.Mvc.Testing;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace WebApplication1.Tests
{
    public class ApiTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> factory;

        /// <summary>
        /// Initialization
        /// </summary>
        public ApiTest(WebApplicationFactory<Program> factory)
        {
            this.factory = factory;
        }

        /// <summary>
        /// GET /WeatherForecast test
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetTest()
        {
            var httpClient = factory.CreateClient();
            
            var response = await httpClient.GetAsync("/WeatherForecast").ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var contentString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            Assert.NotEmpty(contentString);

            var contentModels = JsonSerializer.Deserialize<IReadOnlyList<WeatherForecast>>(contentString);
            Assert.NotNull(contentModels);
            Assert.Equal(5, contentModels!.Count);
            Assert.All(contentModels, model =>
            {
                Assert.True(model.TemperatureC >= -20);
                Assert.True(model.TemperatureC < 55);
            });
        }
    }
}