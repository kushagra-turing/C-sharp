// Controllers/UserController.cs
using Microsoft.AspNetCore.Mvc;
using UserApi.Models;

namespace UserApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpGet("user/{id}")]
        public IActionResult GetUser(int id)
        {
            // Simulate data retrieval based on ID
            if (id <= 0)
            {
                return NotFound();
            }

            var user = new User { Id = id, Name = "Test User", Email = "test@example.com" };
            return Ok(user);
        }

        [HttpGet("user/profile")]
        public IActionResult GetProfile()
        {
            // Simulate profile retrieval
            var profile = new Profile { DisplayName = "Test User Profile", Bio = "A sample bio." };
            return Ok(profile);
        }
    }
}