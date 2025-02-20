using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;  // Add this for HttpRequest
using Microsoft.AspNetCore.Mvc;  // Add this for OkObjectResult
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using NUnit.Framework;
using Moq;

namespace DurableOrchestrationExample
{
    [TestFixture]
    public class OrchestratorTests
    {
        [Test]
        public async Task RunOrchestrator_ExecutesActivities_Successfully()
        {
            // Arrange
            var mockContext = new Mock<IDurableOrchestrationContext>();
            mockContext.Setup(c => c.CallActivityAsync<string>("Hello", null)).ReturnsAsync("Hello");
            mockContext.Setup(c => c.CallActivityAsync<string>("Bye", null)).ReturnsAsync("Bye");
            mockContext.Setup(c => c.InstanceId).Returns("testInstanceId");

            // Act
            await OrchestratorFunctions.RunOrchestrator(mockContext.Object);

            // Assert
            mockContext.Verify(c => c.CallActivityAsync<string>("Hello", null), Times.Exactly(10));
            mockContext.Verify(c => c.CallActivityAsync<string>("Bye", null), Times.Exactly(10));
        }

        [Test]
        public void Hello_Activity_ReturnsExpectedResult()
        {
            // Arrange
            var mockContext = new Mock<IDurableActivityContext>();

            // Act
            string result = OrchestratorFunctions.Hello(mockContext.Object);

            // Assert
            Assert.AreEqual("Hello", result);
        }

        [Test]
        public void Bye_Activity_ReturnsExpectedResult()
        {
            // Arrange
            var mockContext = new Mock<IDurableActivityContext>();

            // Act
            string result = OrchestratorFunctions.Bye(mockContext.Object);

            // Assert
            Assert.AreEqual("Bye", result);
        }

        // Example test for ClientFunctions.StartOrchestration
        [Test]
        public async Task StartOrchestration_StartsNewOrchestration()
        {
            // Arrange
            var mockDurableClient = new Mock<IDurableOrchestrationClient>();
            var mockRequest = new Mock<HttpRequest>();  // Mock the HttpRequest

            string functionName = "TestOrchestrator";
            string expectedInstanceId = Guid.NewGuid().ToString();

            // Change this to return Task<string> instead of Task
            mockDurableClient.Setup(x => x.StartNewAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(expectedInstanceId) // Return a Task<string> here with the instance ID
                .Callback<string, string>((name, id) =>
                {
                    // Capture the instance ID to verify it was generated and used
                    expectedInstanceId = id;
                });

            // Act
            var result = await OrchestratorFunctions.ClientFunctions.StartOrchestration(mockRequest.Object, mockDurableClient.Object, functionName);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.AreEqual(expectedInstanceId, okResult.Value);
            mockDurableClient.Verify(x => x.StartNewAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        // Negative test: RunOrchestrator fails when activity throws an exception
        [Test]
        public async Task RunOrchestrator_ThrowsException_WhenActivityFails()
        {
            // Arrange
            var mockContext = new Mock<IDurableOrchestrationContext>();
            mockContext.Setup(c => c.CallActivityAsync<string>("Hello", null)).ThrowsAsync(new Exception("Activity failed"));
            mockContext.Setup(c => c.CallActivityAsync<string>("Bye", null)).ReturnsAsync("Bye");

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(async () => await OrchestratorFunctions.RunOrchestrator(mockContext.Object));
            Assert.AreEqual("Activity failed", ex.Message);
        }
    }
}
