using AppModels;
using Microsoft.AspNetCore.Mvc;
using WaterJugChallenge.Services;
namespace WaterJugChallenge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WaterJugRiddleController : ControllerBase
    {
        private readonly IWaterJugRiddleService _service;

        public WaterJugRiddleController(IWaterJugRiddleService service)
        {
            _service = service;
        }

        // GET api/waterjugriddle/3/5/4
        [HttpGet("{xCapacity}/{yCapacity}/{zAmountWanted}")]
        public IActionResult GetFromUrl(int xCapacity, int yCapacity, int zAmountWanted)
        {
            // Validate parameters and return ProblemDetails if invalid
            var validationError = ValidateParameters(xCapacity, yCapacity, zAmountWanted);
            if (validationError != null)
            {
                return validationError;
            }

            var result = _service.SolveWaterJugProblem(xCapacity, yCapacity, zAmountWanted);
            return Ok(result);
        }

        // POST api/waterjugriddle
        [HttpPost]
        public IActionResult GetFromBody([FromBody] WaterJugRequest request)
        {
            // Validate parameters and return ProblemDetails if invalid
            var validationError = ValidateParameters(request.XCapacity, request.YCapacity, request.ZAmountWanted);
            if (validationError != null)
            {
                return validationError;
            }

            var result = _service.SolveWaterJugProblem(request.XCapacity, request.YCapacity, request.ZAmountWanted);
            return Ok(result);
        }

        // Utility method to validate parameters
        private IActionResult ValidateParameters(int xCapacity, int yCapacity, int zAmountWanted)
        {
            if (xCapacity <= 0 || yCapacity <= 0 || zAmountWanted <= 0)
            {
                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid Parameters",
                    Detail = "All parameters must be positive integers greater than 0.",
                    Instance = HttpContext.Request.Path
                };
                return BadRequest(problemDetails);
            }
            return null;
        }
    }
}