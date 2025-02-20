using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using RequestCounterApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;

namespace RequestCounterApi.Tests
{
    public class RequestCounterControllerTests
    {
        private RequestCounterController _controller;

        [SetUp]
        public void Setup()
        {
            _controller = new RequestCounterController();

            //Mocking the ControllerContext is crucial for unit testing controllers that rely on properties
            //   provided by the base ControllerBase class.
            var httpContext = new DefaultHttpContext();
            var routeData = new RouteData();
            var actionDescriptor = new ControllerActionDescriptor();

            var controllerContext = new ControllerContext(new ActionContext(httpContext, routeData, actionDescriptor));
            _controller.ControllerContext = controllerContext;
        }

        [Test]
        public void GetRequestCount_ReturnsOkResultWithIncrementedCount()
        {
            // Act
            var result = _controller.GetRequestCount() as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            Assert.IsInstanceOf<int>(result.Value);

            // Call it again and assert the increment
            var secondResult = _controller.GetRequestCount() as OkObjectResult;
            Assert.IsNotNull(secondResult);
            Assert.AreEqual(StatusCodes.Status200OK, secondResult.StatusCode);
            Assert.IsInstanceOf<int>(secondResult.Value);
            Assert.AreEqual((int)result.Value + 1, secondResult.Value); //Ensure that the value increases by one each time.
        }
    }
}