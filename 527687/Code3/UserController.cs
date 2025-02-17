using Microsoft.AspNetCore.Mvc;
using System;

namespace MyWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        // In a real application, you'd likely inject a service or repository
        // to handle data access.  For this example, I'm using simple hardcoded data.

        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            // Simulate retrieving the user profile for the logged-in user.
            var profile = new
            {
                Id = "current_user",
                Name = "John Doe",
                Email = "john.doe@example.com"
            };

            return Ok(profile);
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("User ID cannot be empty.");
            }

            // Simulate retrieving a user by ID.
            if (id == "123")
            {
                var user = new
                {
                    Id = id,
                    Name = "Jane Smith",
                    Email = "jane.smith@example.com"
                };
                return Ok(user);
            }
            else
            {
                return NotFound($"User with ID '{id}' not found.");
            }
        }
    }
}