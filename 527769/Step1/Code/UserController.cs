using Microsoft.AspNetCore.Mvc;
using SmartFormat;
using SmartFormat.Core.Settings;
using System;
using System.Collections.Generic;

namespace YourProjectName.Controllers // Replace with your actual namespace
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        [HttpGet("details")]
        public IActionResult GetUserDetails()
        {
            var user = new UserDetails
            {
                FirstName = "John",
                LastName = "Doe",
                Age = 30,
                Hobbies = new List<string> { "reading", "hiking", "coding" }
            };

            // Configure SmartFormat with pluralization
            var formatter = Smart.CreateDefaultSmartFormat();
            formatter.Settings = new SmartFormatSettings { FormatErrorAction = ErrorAction.ThrowError };

            // Format the output string
            string formatString = "User: {FirstName} {LastName}, Age: {Age}, Hobby{Hobbies.Count:s||ies}: {Hobbies:list:|, }"; // Pluralization here
            string formattedString = formatter.Format(formatString, user);

            Console.WriteLine(formattedString);  // Output to console

            return Ok(new { Message = "User details printed to console." }); // Return a simple response
        }
    }
}