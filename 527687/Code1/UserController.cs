using Microsoft.AspNetCore.Mvc;

namespace UserApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpGet("user/{id}")]
        public IActionResult GetUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("User ID is required.");
            }

            var user = _userService.GetUserById(id);

            if (user == null)
            {
                return NotFound($"User with ID '{id}' not found.");
            }

            return Ok(user);
        }

        [HttpGet("user/profile")]
        public IActionResult GetProfile()
        {
            var profile = _userService.GetUserProfile();

            if (profile == null)
            {
                return NotFound("User profile not found.");
            }

            return Ok(profile);
        }
    }
}