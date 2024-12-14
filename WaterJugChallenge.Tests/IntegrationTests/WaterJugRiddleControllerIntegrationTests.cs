using AppModels;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
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
            response.IsSuccessStatusCode.Should().BeTrue(); // Checks that the response status code is between 200-299
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<SolutionResponse>(content);

            result.Should().NotBeNull();
            result.Solution.Should().HaveCount(6); // We expect 6 steps in the solution
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
            response.IsSuccessStatusCode.Should().BeTrue(); // Status code 200-299 indicates success
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<SolutionResponse>(responseContent);

            result.Should().NotBeNull();
            result.Solution.Should().HaveCount(6); // Expect 6 steps in the solution
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
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest); // Check for 400 BadRequest status code
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("Invalid Parameters");
        }

        // Test for missing parameters in GET
        [Fact]
        public async Task GetFromUrl_MissingParameter_ReturnsBadRequest()
        {
            // Arrange
            var xCapacity = 3;
            var yCapacity = 5;

            // Act
            var response = await _client.GetAsync($"/api/waterjugriddle/{xCapacity}/{yCapacity}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound); // Should return 404 NotFound
            var content = await response.Content.ReadAsStringAsync();
            content.Should().BeEmpty();
        }
    }
}
