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

        // GET api/waterjugriddle/3/5/0
        [HttpGet("{xCapacity}/{yCapacity}/{zAmountWanted}")]
        public IActionResult GetFromUrl(int xCapacity, int yCapacity, int zAmountWanted)
        {
            // Validate that all parameters are positive integers greater than 0
            if (xCapacity <= 0 || yCapacity <= 0 || zAmountWanted <= 0 || !ModelState.IsValid)
            {
                throw new ArgumentException("All parameters must be positive integers greater than 0.");
            }

            var result = _service.SolveWaterJugProblem(xCapacity, yCapacity, zAmountWanted);
            return Ok(result);
        }


        // POST api/waterjugriddle
        [HttpPost]
        public IActionResult GetFromBody([FromBody] WaterJugRequest request)
        {
            // Validate the request body parameters
            if (request.XCapacity <= 0 || request.YCapacity <= 0 || request.ZAmountWanted <= 0)
            {
                throw new ArgumentException("All parameters must be positive integers greater than 0.");
            }

            var result = _service.SolveWaterJugProblem(request.XCapacity, request.YCapacity, request.ZAmountWanted);
            return Ok(result);
        }
    }
}
