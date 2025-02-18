using System;
using System.Threading.Tasks;
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
            mockContext.Setup(c => c.CallActivityAsync<string>("Hello")).ReturnsAsync("Hello from activity!");
            mockContext.Setup(c => c.CallActivityAsync<string>("Bye")).ReturnsAsync("Bye from activity!");
            mockContext.Setup(c => c.InstanceId).Returns("testInstanceId");

            // Act
            await OrchestratorFunctions.RunOrchestrator(mockContext.Object);

            // Assert
            mockContext.Verify(c => c.CallActivityAsync<string>("Hello"), Times.Once);
            mockContext.Verify(c => c.CallActivityAsync<string>("Bye"), Times.Once);
        }

        [Test]
        public void Hello_Activity_ReturnsExpectedResult()
        {
            // Arrange
            var mockContext = new Mock<IDurableActivityContext>();

            // Act
            string result = OrchestratorFunctions.Hello(mockContext.Object);

            // Assert
            Assert.AreEqual("Hello from activity!", result);
        }

        [Test]
        public void Bye_Activity_ReturnsExpectedResult()
        {
            // Arrange
            var mockContext = new Mock<IDurableActivityContext>();

            // Act
            string result = OrchestratorFunctions.Bye(mockContext.Object);

            // Assert
            Assert.AreEqual("Bye from activity!", result);
        }

        //Example test for ClientFunctions.StartOrchestration
        [Test]
        public async Task StartOrchestration_StartsNewOrchestration()
        {
            // Arrange
            var mockDurableClient = new Mock<IDurableOrchestrationClient>();
            var mockRequest = new Mock<HttpRequest>();

            string functionName = "TestOrchestrator";
            string expectedInstanceId = Guid.NewGuid().ToString();

            mockDurableClient.Setup(x => x.StartNewAsync(functionName, It.IsAny<string>()))
               .Returns(Task.CompletedTask)
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
            mockDurableClient.Verify(x => x.StartNewAsync(functionName, It.IsAny<string>()), Times.Once);
        }

    }
}