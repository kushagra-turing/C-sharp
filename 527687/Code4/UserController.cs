// UserApi/Controllers/UserController.cs
using Microsoft.AspNetCore.Mvc;
using UserApi.Models;

namespace UserApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        // In a real application, this would come from a database or service.
        static List<User> Users = new List<User>()
        {
            new User { Id = "1", Name = "John Doe", Email = "john.doe@example.com" },
            new User { Id = "2", Name = "Jane Smith", Email = "jane.smith@example.com" }
        };

        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }


        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            // Assuming we want to return the profile of user with id "1"
            var user = Users.FirstOrDefault(u => u.Id == "1");

            if (user == null)
            {
                return NotFound("Profile not found");
            }

            return Ok(user);
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("User ID cannot be empty.");
            }

            var user = Users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound($"User with ID '{id}' not found.");
            }

            return Ok(user);
        }
    }
}