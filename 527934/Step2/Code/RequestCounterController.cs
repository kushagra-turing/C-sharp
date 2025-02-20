// Code: ASP.NET Core API (RequestCounterController.cs)
using Microsoft.AspNetCore.Mvc;

namespace RequestCounterApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RequestCounterController : ControllerBase
    {
        private static int _requestCount = 0;
        private static readonly object _lock = new object(); // Ensure thread safety

        [HttpGet("count")]
        public IActionResult GetRequestCount()
        {
            lock (_lock)
            {
                _requestCount++;
                return Ok(_requestCount);
            }
        }
    }
}