using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using UserApi.Controllers;
using Microsoft.Extensions.Logging;
using UserApi.Models;
using Moq;

namespace UserApi.Tests
{
    public class UserControllerTests
    {
        private UserController _controller;

        [SetUp]
        public void Setup()
        {
            // Mock the logger
            var mockLogger = new Mock<ILogger<UserController>>();
            _controller = new UserController(mockLogger.Object);
        }

        [Test]
        public void GetUser_ValidId_ReturnsOkResult()
        {
            // Act
            var result = _controller.GetUser(1) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            var user = result.Value as User;
            Assert.IsNotNull(user);
            Assert.AreEqual(1, user.Id);
        }

        [Test]
        public void GetUser_InvalidId_ReturnsNotFoundResult()
        {
            // Act
            var result = _controller.GetUser(-1) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }

        [Test]
        public void GetProfile_ReturnsOkResult()
        {
            // Act
            var result = _controller.GetProfile() as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            var profile = result.Value as Profile;
            Assert.IsNotNull(profile);
            Assert.IsNotEmpty(profile.DisplayName);
        }
    }
}