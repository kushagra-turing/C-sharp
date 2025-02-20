using Microsoft.AspNetCore.Mvc;

namespace RequestCounterApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RequestCounterController : ControllerBase
    {
        private static int _requestCount = 0;

        [HttpGet("count")]
        public IActionResult GetRequestCount()
        {
            _requestCount++;
            return Ok(_requestCount);
        }
    }
}