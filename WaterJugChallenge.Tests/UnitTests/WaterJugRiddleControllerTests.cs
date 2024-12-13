using AppModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WaterJugChallenge.Controllers;
using WaterJugChallenge.Services;
using Xunit;

namespace WaterJugChallenge.Tests.Controllers
{
    public class WaterJugRiddleControllerTests
    {
        private readonly Mock<IWaterJugRiddleService> _mockService;
        private readonly WaterJugRiddleController _controller;

        public WaterJugRiddleControllerTests()
        {
            // Initialize mock service and controller
            _mockService = new Mock<IWaterJugRiddleService>();
            _controller = new WaterJugRiddleController(_mockService.Object);
        }

        // Test the GET method with valid parameters
        [Fact]
        public void GetFromUrl_ValidParameters_ReturnsOkResultWithSolution()
        {
            // Arrange
            int xCapacity = 3;
            int yCapacity = 5;
            int zAmountWanted = 4;


            // Expected result
            var expectedResult = new SolutionResponse
            {
                Solution = new List<Step>
                {
                    new Step { StepNumber = 1, BucketX = 2, BucketY = 0, Action = "Fill bucket X" },
                    new Step { StepNumber = 2, BucketX = 0, BucketY = 2, Action = "Transfer from bucket X to Y" },
                    new Step { StepNumber = 3, BucketX = 2, BucketY = 2, Action = "Fill bucket X" },
                    new Step { StepNumber = 4, BucketX = 0, BucketY = 4, Action = "Transfer from bucket X to Y", Status = "Solved" }
                }
            };

            // Mock the service method to return the expected result
            _mockService.Setup(s => s.SolveWaterJugProblem(xCapacity, yCapacity, zAmountWanted))
                        .Returns(expectedResult);

            // Act
            var result = _controller.GetFromUrl(xCapacity, yCapacity, zAmountWanted);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);  // Check if result is OK
            var actualResult = Assert.IsType<SolutionResponse>(okResult.Value); // Check if the result is of type SolutionResponse
            Assert.Equal(expectedResult.Solution.Count, actualResult.Solution.Count);  // Check if the count matches

            // Check if the first step matches the expected result
            Assert.Equal(expectedResult.Solution[0].StepNumber, actualResult.Solution[0].StepNumber);
            Assert.Equal(expectedResult.Solution[0].BucketX, actualResult.Solution[0].BucketX);
            Assert.Equal(expectedResult.Solution[0].BucketY, actualResult.Solution[0].BucketY);
            Assert.Equal(expectedResult.Solution[0].Action, actualResult.Solution[0].Action);
            Assert.Equal(expectedResult.Solution[0].Status, actualResult.Solution[0].Status);
        }

        // Test the GET method with invalid parameters (zero for zAmountWanted)
        [Fact]
        public void GetFromUrl_InvalidParameters_ReturnsBadRequest()
        {
            // Arrange
            int xCapacity = 3;
            int yCapacity = 5;
            int zAmountWanted = 0; // Invalid value

            // Act
            var result = _controller.GetFromUrl(xCapacity, yCapacity, zAmountWanted);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);  // Check if it's a BadRequest response
            Assert.Equal("All parameters must be positive integers.", badRequestResult.Value);  // Verify error message
        }

        // Test the POST method with valid parameters
        [Fact]
        public void GetFromBody_ValidParameters_ReturnsOkResultWithSolution()
        {
            // Arrange
            var request = new WaterJugRequest
            {
                XCapacity = 3,
                YCapacity = 5,
                ZAmountWanted = 4
            };

            var expectedResult = new SolutionResponse
            {
                Solution = new List<Step>
                {
                    new Step { StepNumber = 1, BucketX = 3, BucketY = 0, Action = "Fill Jug X", Status = "In progress" },
                    new Step { StepNumber = 2, BucketX = 0, BucketY = 5, Action = "Fill Jug Y", Status = "In progress" },
                    new Step { StepNumber = 3, BucketX = 3, BucketY = 2, Action = "Pour Jug Y into Jug X", Status = "Complete" }
                }
            };

            // Mock the service method to return the expected result
            _mockService.Setup(s => s.SolveWaterJugProblem(request.XCapacity, request.YCapacity, request.ZAmountWanted))
                        .Returns(expectedResult);

            // Act
            var result = _controller.GetFromBody(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);  // Check if result is OK
            var actualResult = Assert.IsType<SolutionResponse>(okResult.Value); // Check if the result is of type SolutionResponse

            // Debug output to check what the actual result is
            Console.WriteLine("Actual result:");
            foreach (var step in actualResult.Solution)
            {
                Console.WriteLine($"Step: {step.StepNumber}, Action: {step.Action}, BucketX: {step.BucketX}, BucketY: {step.BucketY}");
            }

            Assert.Equal(expectedResult.Solution.Count, actualResult.Solution.Count);  // Check if the count matches

            // Check if the first step matches the expected result
            Assert.Equal(expectedResult.Solution[0].StepNumber, actualResult.Solution[0].StepNumber);
            Assert.Equal(expectedResult.Solution[0].BucketX, actualResult.Solution[0].BucketX);
            Assert.Equal(expectedResult.Solution[0].BucketY, actualResult.Solution[0].BucketY);
            Assert.Equal(expectedResult.Solution[0].Action, actualResult.Solution[0].Action);
            Assert.Equal(expectedResult.Solution[0].Status, actualResult.Solution[0].Status);
        }


        // Test the POST method with invalid parameters (zero for zAmountWanted)
        [Fact]
        public void GetFromBody_InvalidParameters_ReturnsBadRequest()
        {
            // Arrange
            var request = new WaterJugRequest
            {
                XCapacity = 3,
                YCapacity = 5,
                ZAmountWanted = 0  // Invalid value
            };

            // Act
            var result = _controller.GetFromBody(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);  // Check if it's a BadRequest response
            Assert.Equal("All parameters must be positive integers.", badRequestResult.Value);  // Verify error message
        }
    }
}
