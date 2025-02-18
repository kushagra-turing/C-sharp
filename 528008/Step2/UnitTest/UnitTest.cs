using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using NUnit.Framework;
using System.Threading;

namespace DurableTaskExample
{
    [TestFixture]
    public class OrchestratorTests
    {
        [Test]
        public async Task TestOrchestrator()
        {
            // Arrange
            string instanceId = Guid.NewGuid().ToString();
            var durableClient = new MockDurableOrchestrationClient();

            // Act
            // Start the orchestrator
            await OrchestratorExample.RunClient(durableClient, instanceId);
            // Simulate re-running the orchestrator every 1 second for 10 seconds.
            for (int i = 0; i < 10; i++)
            {
                 await Task.Delay(1000);
                 await OrchestratorExample.RunClient(durableClient, instanceId);
            }


            // Assert
            // You would need to implement methods in MockDurableOrchestrationClient
            // to track the calls to CallActivityAsync and other Durable Functions methods.
            // Then, you can assert that the methods were called with the expected parameters
            // and in the expected order.

            // For example:
            Assert.That(durableClient.StartNewAsyncCallCount, Is.GreaterThanOrEqualTo(1)); //Orchestrator started at least once
            Assert.That(durableClient.CallActivityAsyncCallCount, Is.GreaterThanOrEqualTo(2)); //Hello and Bye activities called at least once

            // More specific assertions would depend on the implementation of MockDurableOrchestrationClient.
        }
    }
}