using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SmartFormat;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;

namespace UserDetailsApi.Controllers
{
    [TestFixture]
    public class UserControllerTests
    {
        [Test]
        public void GetUserDetails_PrintsFormattedUserDetailsToConsole()
        {
            // Arrange
            var controller = new UserController();

            // Capture console output
            using (var consoleOutput = new StringWriter())
            {
                Console.SetOut(consoleOutput);

                // Act
                var result = controller.GetUserDetails();

                // Assert
                Assert.IsInstanceOf<OkResult>(result);
                string expectedOutput = "User: John Doe, Age: 30, Hobbies: 3 hobbies" + Environment.NewLine; // Include newline character
                Assert.AreEqual(expectedOutput, consoleOutput.ToString());
            }
            // Reset console output
            Console.SetOut(Console.Out);
        }

        [SetUp]
        public void Setup()
        {
            //Needed for controller context
            var httpContext = new DefaultHttpContext();
            var controllerContext = new ControllerContext(new ActionContext(httpContext, new RouteData(), new ControllerActionDescriptor()));
            
        }
    }
}