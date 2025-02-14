using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using UserApi.Controllers;
using UserApi.Services;
using Moq;  // Install Moq:  dotnet add package Moq

namespace UserApi.Tests
{
    public class UserControllerTests
    {
        private Mock<IUserService> _mockUserService;
        private UserController _controller;

        [SetUp]
        public void Setup()
        {
            _mockUserService = new Mock<IUserService>();
            _controller = new UserController(_mockUserService.Object);
        }

        [Test]
        public void GetUser_ExistingId_ReturnsOkResultWithUser()
        {
            // Arrange
            var expectedUser = new User { Id = 1, Name = "Test User", Email = "test@example.com" };
            _mockUserService.Setup(service => service.GetUserById(1)).Returns(expectedUser);

            // Act
            var result = _controller.GetUser(1) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(expectedUser, result.Value);
        }

        [Test]
        public void GetUser_NonExistingId_ReturnsNotFoundResult()
        {
            // Arrange
            _mockUserService.Setup(service => service.GetUserById(99)).Returns((User)null); // Returns null

            // Act
            var result = _controller.GetUser(99) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }

        [Test]
        public void GetProfile_ProfileExists_ReturnsOkResultWithProfile()
        {
            // Arrange
            var expectedProfile = new UserProfile { Bio = "Test Bio", Interests = new List<string> { "Reading" } };
            _mockUserService.Setup(service => service.GetUserProfile()).Returns(expectedProfile);

            // Act
            var result = _controller.GetProfile() as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(expectedProfile, result.Value);
        }

        [Test]
        public void GetProfile_ProfileDoesNotExist_ReturnsNotFoundResult()
        {
            // Arrange
            _mockUserService.Setup(service => service.GetUserProfile()).Returns((UserProfile)null);

            // Act
            var result = _controller.GetProfile() as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }
    }
}