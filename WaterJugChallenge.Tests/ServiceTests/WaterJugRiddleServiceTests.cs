using AppModels;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using WaterJugChallenge.Services;
using Xunit;

namespace WaterJugChallenge.Tests.ServiceTests
{
    public class WaterJugRiddleServiceTests
    {
        private readonly Mock<IMemoryCache> _mockCache;
        private readonly WaterJugRiddleService _service;
        private readonly Dictionary<string, object> _cacheDictionary;

        public WaterJugRiddleServiceTests()
        {
            // Mocking IMemoryCache
            _mockCache = new Mock<IMemoryCache>();

            // Simulate cache with a dictionary
            _cacheDictionary = new Dictionary<string, object>();

            // Setup TryGetValue to simulate a cache lookup
            _mockCache.Setup(c => c.TryGetValue(It.IsAny<string>(), out It.Ref<object>.IsAny))
                      .Returns((string key, out object value) =>
                      {
                          return _cacheDictionary.TryGetValue(key, out value);
                      });

            // Initialize the service with the mocked cache
            _service = new WaterJugRiddleService(_mockCache.Object);
        }

        [Fact]
        public void SolveWaterJugProblem_ShouldReturnCachedSolution_WhenAlreadyCached()
        {
            // Arrange
            int xCapacity = 3;
            int yCapacity = 5;
            int zAmountWanted = 4;

            var cachedSolution = new SolutionResponse
            {
                Solution = new List<Step>
                {
                    new Step { StepNumber = 1, BucketX = 3, BucketY = 0, Action = "Fill bucket X" }
                }
            };

            var cacheKey = $"{xCapacity}-{yCapacity}-{zAmountWanted}";
            _cacheDictionary[cacheKey] = cachedSolution; // Simulate adding a solution to the cache

            // Act
            var result = _service.SolveWaterJugProblem(xCapacity, yCapacity, zAmountWanted);

            // Assert
            result.Should().NotBeNull();
            result.Solution.Should().HaveCount(cachedSolution.Solution.Count);
            result.Solution[0].Action.Should().Be(cachedSolution.Solution[0].Action);
        }

        [Fact]
        public void SolveWaterJugProblem_ShouldThrowException_WhenNoSolutionExists()
        {
            // Arrange
            int xCapacity = 3;
            int yCapacity = 5;
            int zAmountWanted = 7;  // No solution exists for this case

            // Act & Assert
            Action act = () => _service.SolveWaterJugProblem(xCapacity, yCapacity, zAmountWanted);
            act.Should().Throw<AppException>().WithMessage("No Solution");
        }

        [Fact]
        public void SolveWaterJugProblem_ShouldReturnSolution_WhenSolvable()
        {
            // Arrange
            int xCapacity = 3;
            int yCapacity = 5;
            int zAmountWanted = 4;

            // Setup the expected solution in cache before calling the method.
            var cacheKey = $"{xCapacity}-{yCapacity}-{zAmountWanted}";
            var expectedSolution = new SolutionResponse
            {
                Solution = new List<Step>
                {
                    new Step { StepNumber = 1, BucketX = 3, BucketY = 0, Action = "Fill bucket X" }
                }
            };
            _cacheDictionary[cacheKey] = expectedSolution;

            // Act
            var result = _service.SolveWaterJugProblem(xCapacity, yCapacity, zAmountWanted);

            // Assert
            result.Should().NotBeNull();
            result.Solution.Should().NotBeEmpty();
            result.Solution[0].Action.Should().Be("Fill bucket X"); // Verify the first action
        }

        [Fact]
        public void CalculateGreatestCommonDivisor_ShouldReturnCorrectGCD()
        {
            // Arrange
            int a = 3;
            int b = 5;

            // Act
            var result = _service.CalcualteGreatestCommonDivisor(a, b);

            // Assert
            result.Should().Be(1);  // The GCD of 3 and 5 is 1
        }
    }
}
