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

        // GET api/waterjugriddle/2/10/4
        [HttpGet("{xCapacity}/{yCapacity}/{zAmountWanted}")]
        public IActionResult GetFromUrl(int xCapacity, int yCapacity, int zAmountWanted)
        {
            var result = _service.SolveWaterJugProblem(xCapacity, yCapacity, zAmountWanted);
            return Ok(result);
        }

        // POST api/waterjugriddle
        [HttpPost]
        public IActionResult GetFromBody([FromBody] WaterJugRequest request)
        {
            var result = _service.SolveWaterJugProblem(request.XCapacity, request.YCapacity, request.ZAmountWanted);
            return Ok(result);
        }
    }
}
