namespace WaterJugChallenge.IntegrationTests
{
    public class WaterJugRiddleControllerIntegrationTests : IClassFixture<WebApplicationFactory<WaterJugChallenge.Startup>>
    {
        private readonly WebApplicationFactory<WaterJugChallenge.Startup> _factory;
        private readonly HttpClient _client;

        public WaterJugRiddleControllerIntegrationTests(WebApplicationFactory<WaterJugChallenge.Startup> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task GetFromUrl_ValidParameters_ReturnsOkResult()
        {
            // Your integration test logic here
        }
    }
}
