// UserApi.Tests/UserControllerTests.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using UserApi.Controllers;
using UserApi.Models;

namespace UserApi.Tests
{
    public class UserControllerTests
    {
        private Mock<ILogger<UserController>> _loggerMock;
        private UserController _controller;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<UserController>>();
            _controller = new UserController(_loggerMock.Object);
        }

        [Test]
        public void GetUser_ValidId_ReturnsOkResult()
        {
            // Arrange
            string validId = "1";

            // Act
            var result = _controller.GetUser(validId) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            var user = result.Value as User;
            Assert.IsNotNull(user);
            Assert.AreEqual(validId, user.Id);
        }

        [Test]
        public void GetUser_InvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            string invalidId = "99";

            // Act
            var result = _controller.GetUser(invalidId);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public void GetUser_EmptyId_ReturnsBadRequestResult()
        {
            // Arrange
            string emptyId = "";

            // Act
            var result = _controller.GetUser(emptyId);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void GetProfile_ReturnsOkResult()
        {
            // Act
            var result = _controller.GetProfile() as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            var user = result.Value as User;
            Assert.IsNotNull(user);
            Assert.AreEqual("1", user.Id); // Ensure it returns the profile of user "1"
        }

        [Test]
        public void GetProfile_ProfileNotFound_ReturnsNotFoundResult()
        {
            // Arrange
            //Simulate no user exists. You would typically mock the underlying data service for this.
            var loggerMock = new Mock<ILogger<UserController>>();
            var controller = new UserController(loggerMock.Object);

            // Act - modify internal state using reflection to simulate user not existing.
            var fieldInfo = typeof(UserController).GetField("Users", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            var originalUsers = fieldInfo.GetValue(null); // Save original
            fieldInfo.SetValue(null, new List<User>()); // Clear users

            var result = controller.GetProfile();

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);

            //Cleanup - restore original state
            fieldInfo.SetValue(null, originalUsers);

        }
    }
}