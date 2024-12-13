using AppModels;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Text;
using Xunit;

namespace WaterJugChallenge.Tests.Integration
{
    public class WaterJugRiddleControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;

        public WaterJugRiddleControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();  // Creates an in-memory HttpClient to send requests to the server
        }

        // Test for the GET endpoint
        [Fact]
        public async Task GetFromUrl_ValidParameters_ReturnsOkResultWithSolution()
        {
            // Arrange
            var xCapacity = 3;
            var yCapacity = 5;
            var zAmountWanted = 4;

            // Act
            var response = await _client.GetAsync($"/api/waterjugriddle/{xCapacity}/{yCapacity}/{zAmountWanted}");

            // Assert
            response.EnsureSuccessStatusCode(); // Status code 200-299 indicates success
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<SolutionResponse>(content);

            Assert.NotNull(result);
            Assert.Equal(6, result.Solution.Count); // We expect 3 steps in the solution
        }

        // Test for the POST endpoint
        [Fact]
        public async Task GetFromBody_ValidParameters_ReturnsOkResultWithSolution()
        {
            // Arrange
            var request = new WaterJugRequest
            {
                XCapacity = 3,
                YCapacity = 5,
                ZAmountWanted = 4
            };

            var jsonContent = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/waterjugriddle", content);

            // Assert
            response.EnsureSuccessStatusCode();  // Status code 200-299 indicates success
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<SolutionResponse>(responseContent);

            Assert.NotNull(result);
            Assert.Equal(6, result.Solution.Count); // Expect 3 steps
        }

        // Test for invalid parameters in GET
        [Fact]
        public async Task GetFromUrl_InvalidParameters_ReturnsBadRequest()
        {
            // Arrange
            var xCapacity = 3;
            var yCapacity = 5;
            var zAmountWanted = 0; // Invalid zAmountWanted

            // Act
            var response = await _client.GetAsync($"/api/waterjugriddle/{xCapacity}/{yCapacity}/{zAmountWanted}");

            // Assert
            Assert.Equal(400, (int)response.StatusCode); // BadRequest status code
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Invalid Parameters", content);
        }

        // Test for missing parameters in GET
        [Fact]
        public async Task GetFromUrl_MissingParameter_ReturnsBadRequest()
        {
            // Arrange
            var xCapacity = 3;
            var yCapacity = 5;

            var response = await _client.GetAsync($"/api/waterjugriddle/{xCapacity}/{yCapacity}");

            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("", content);
        }
    }
}
