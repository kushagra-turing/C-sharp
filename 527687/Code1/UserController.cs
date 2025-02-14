using Microsoft.AspNetCore.Mvc;

namespace UserApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;  // Added service layer

        public UserController(IUserService userService) // Use Dependency Injection
        {
            _userService = userService;
            Console.WriteLine("Added controller");
        }

        [HttpGet("user/{id}")]
        public IActionResult GetUser(int id)
        {
            var user = _userService.GetUserById(id);  // Delegate to service

            if (user == null)
            {
                return NotFound(); // Return 404 if not found
            }

            return Ok(user); // Return 200 with user data
        }

        [HttpGet("user/profile")]
        public IActionResult GetProfile()
        {
            var profile = _userService.GetUserProfile(); //Delegate to service

            if (profile == null)
            {
                return NotFound(); // or perhaps NoContent() if appropriate
            }

            return Ok(profile);
        }
    }
}