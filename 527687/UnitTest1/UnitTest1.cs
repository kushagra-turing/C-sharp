using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using UserApi.Controllers;
using UserApi.Models;
using UserApi.Services;

namespace UserApi.Tests
{
    public class UserControllerTests
    {
        private UserController _controller;
        private MockUserService _mockUserService;

        [SetUp]
        public void Setup()
        {
            _mockUserService = new MockUserService();
            _controller = new UserController(_mockUserService);
        }

        [Test]
        public void GetUser_ValidId_ReturnsOkResultWithUser()
        {
            // Arrange
            _mockUserService.SetupGetUserById("1", new User { Id = "1", Name = "Test User" });

            // Act
            var result = _controller.GetUser("1") as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsInstanceOf<User>(result.Value);
            Assert.AreEqual("Test User", ((User)result.Value).Name);
        }

        [Test]
        public void GetUser_InvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            _mockUserService.SetupGetUserById("3", null);

            // Act
            var result = _controller.GetUser("3") as NotFoundObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }

        [Test]
        public void GetUser_EmptyId_ReturnsBadRequestResult()
        {
            // Act
            var result = _controller.GetUser("") as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual("User ID is required.", result.Value);
        }

        [Test]
        public void GetProfile_ProfileExists_ReturnsOkResultWithProfile()
        {
            // Arrange
            _mockUserService.SetupGetUserProfile(new UserProfile { FirstName = "Test", LastName = "Profile" });

            // Act
            var result = _controller.GetProfile() as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsInstanceOf<UserProfile>(result.Value);
            Assert.AreEqual("Test", ((UserProfile)result.Value).FirstName);
        }

        [Test]
        public void GetProfile_ProfileDoesNotExist_ReturnsNotFoundResult()
        {
            // Arrange
            _mockUserService.SetupGetUserProfile(null);

            // Act
            var result = _controller.GetProfile() as NotFoundObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }

        // MockUserService for testing
        private class MockUserService : IUserService
        {
            private User? _user;
            private UserProfile? _userProfile;

            public void SetupGetUserById(string id, User? user)
            {
                _user = user;
            }

            public void SetupGetUserProfile(UserProfile? userProfile)
            {
                _userProfile = userProfile;
            }

            public User? GetUserById(string id)
            {
                return _user;
            }

            public UserProfile? GetUserProfile()
            {
                return _userProfile;
            }
        }
    }
}