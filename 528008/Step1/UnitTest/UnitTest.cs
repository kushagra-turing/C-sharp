using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace DurableTaskOrchestrator
{
    [TestFixture]
    public class OrchestratorTests
    {
        [Test]
        public async Task RunOrchestrator_ExecutesActivitiesAndLogsMessages()
        {
            // Arrange
            var contextMock = new MockDurableOrchestrationContext();
            contextMock.Setup(c => c.CallActivityAsync<string>("Hello")).ReturnsAsync("Hello!");
            contextMock.Setup(c => c.CallActivityAsync<string>("Bye")).ReturnsAsync("Bye!");

            var output = new List<string>();
            Console.SetOut(new TestConsole(output)); // Redirect Console.WriteLine

            // Act
            await OrchestratorFunctions.RunOrchestrator(contextMock.Object);

            // Assert
            Assert.That(output.Count, Is.EqualTo(5)); // 5 Console.WriteLine calls
            Assert.That(output[0], Is.EqualTo("Orchestrator started."));
            Assert.That(output[1], Is.EqualTo("Hello!"));
            Assert.That(output[2], Is.EqualTo("Bye!"));
            Assert.That(output[3], Is.EqualTo("Orchestrator finished."));
            Console.SetOut(Console.Out); // Reset Console.WriteLine
        }

        [Test]
        public async Task TimerStarter_StartsOrchestratorMultipleTimes()
        {
            // Arrange
            var durableClient = new TestDurableOrchestrationClient();
            var loggerMock = new Mock<ILogger>();
            int numberOfRuns = 3; // Simulate running the timer 3 times.

            // Act
            for (int i = 0; i < numberOfRuns; i++)
            {
                await OrchestratorFunctions.TimerStarter(new TimerInfo(null, null), durableClient, loggerMock.Object);
                await Task.Delay(1000); // Simulate 1-second delay between runs.
            }


            // Assert
            Assert.That(durableClient.Output.Count, Is.EqualTo(numberOfRuns));
            for (int i = 0; i < numberOfRuns; i++)
            {
                Assert.That(durableClient.Output[i], Does.StartWith("Started orchestration: RunOrchestrator with instance ID: "));
            }

        }
    }

    // Helper class for capturing Console.WriteLine output in tests
    public class TestConsole : TextWriter
    {
        private readonly List<string> _output;

        public TestConsole(List<string> output)
        {
            _output = output;
        }

        public override void WriteLine(string value)
        {
            _output.Add(value);
            base.WriteLine(value);
        }

        public override System.Text.Encoding Encoding => System.Text.Encoding.UTF8;
    }
}