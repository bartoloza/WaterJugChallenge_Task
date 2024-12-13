namespace WaterJugChallenge.Tests.Controllers
{
    public class WaterJugRiddleControllerTests
    {
        private readonly Mock<IWaterJugRiddleService> _mockService;
        private readonly WaterJugRiddleController _controller;

        public WaterJugRiddleControllerTests()
        {
            _mockService = new Mock<IWaterJugRiddleService>();
            _controller = new WaterJugRiddleController(_mockService.Object);
        }

        [Fact]
        public void GetFromUrl_ValidParameters_ReturnsOkResult()
        {
            // Your unit test logic here
        }
    }
}
