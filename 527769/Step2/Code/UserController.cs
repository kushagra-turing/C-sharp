using Microsoft.AspNetCore.Mvc;
using SmartFormat;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;

namespace UserDetailsApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        [HttpGet("details")]
        public IActionResult GetUserDetails()
        {
            // 1. Create a User object with sample data
            var user = new User
            {
                Name = "John Doe",
                Age = 30,
                Hobbies = new string[] { "reading", "hiking", "coding" }
            };

            // 2. Format the user details using SmartFormat
            string format = "User: {Name}, Age: {Age}, Hobbies: {Hobbies:plural(one={#} hobby,other={#} hobbies)}";
            string formattedString = Smart.Format(format, user);

            // 3. Print the formatted string to the console
            System.Console.WriteLine(formattedString);

            // 4. Return an OkResult
            return Ok();
        }
    }

    // Define the User class
    public class User
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string[] Hobbies { get; set; }
    }
}