using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using MyWebApi.Controllers;

namespace MyWebApi.Tests
{
    public class UserControllerTests
    {
        private UserController _controller;

        [SetUp]
        public void Setup()
        {
            _controller = new UserController();
        }

        [Test]
        public void GetProfile_ReturnsOkResult()
        {
            var result = _controller.GetProfile() as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public void GetProfile_ReturnsCorrectProfileData()
        {
            var result = _controller.GetProfile() as OkObjectResult;
            dynamic profile = result.Value;

            Assert.AreEqual("John Doe", profile.Name);
            Assert.AreEqual("john.doe@example.com", profile.Email);
        }

        [Test]
        public void GetUser_ValidId_ReturnsOkResult()
        {
            var result = _controller.GetUser("123") as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public void GetUser_ValidId_ReturnsCorrectUserData()
        {
            var result = _controller.GetUser("123") as OkObjectResult;
            dynamic user = result.Value;

            Assert.AreEqual("Jane Smith", user.Name);
            Assert.AreEqual("jane.smith@example.com", user.Email);
        }

        [Test]
        public void GetUser_InvalidId_ReturnsNotFoundResult()
        {
            var result = _controller.GetUser("456") as NotFoundObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }

        [Test]
        public void GetUser_EmptyId_ReturnsBadRequestResult()
        {
            var result = _controller.GetUser("") as BadRequestObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);

            result = _controller.GetUser(null) as BadRequestObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }
    }
}