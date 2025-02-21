using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using YourProjectName.Controllers; // Replace with your namespace
using Microsoft.AspNetCore.Mvc;

namespace YourProjectName.Tests // Replace with your namespace
{
    [TestFixture]
    public class UserControllerTests
    {
        private WebApplicationFactory<YourProjectName.Startup> _factory; // Replace with your startup class
        private HttpClient _client;

        [SetUp]
        public void Setup()
        {
            _factory = new WebApplicationFactory<YourProjectName.Startup>() // Replace with your startup class
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureLogging(logging =>
                    {
                        logging.ClearProviders(); // Remove default logging providers
                    });
                });

            _client = _factory.CreateClient();
        }


        [TearDown]
        public void TearDown()
        {
            _client.Dispose();
            _factory.Dispose();
        }


        [Test]
        public async Task GetUserDetails_ReturnsOkResult()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/user/details"); // Adjust the URL if needed

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }


        [Test]
        public async Task GetUserDetails_PrintsToConsole()
        {
            // Arrange
            using var stringWriter = new StringWriter();
            Console.SetOut(stringWriter); // Redirect console output to the stringWriter

            var client = _factory.CreateClient();

            // Act
            await client.GetAsync("/user/details");

            // Assert
            var output = stringWriter.ToString().Trim();

            Assert.IsTrue(output.Contains("User: John Doe, Age: 30, Hobbies: reading, hiking, coding"), "Console output does not match expected format.");

            // Restore original console output
            Console.SetOut(Console.Out);
        }



    }
}