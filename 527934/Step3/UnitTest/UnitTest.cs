using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using RequestCounterApi.Controllers;
using System.Threading.Tasks;

namespace RequestCounterApiTests
{
    public class RequestCounterControllerTests
    {
        [SetUp]
        public void Setup()
        {
            // Reset the counter before each test.  Important for isolated tests.
            typeof(RequestCounterController).GetField("_requestCount", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)
                .SetValue(null, 0); // Resets the static field to 0
        }

        [Test]
        public void GetRequestCount_ReturnsOkResultWithIncrementedCount()
        {
            var controller = new RequestCounterController();
            var result = controller.GetRequestCount() as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(1, result.Value);
        }

        [Test]
        public async Task GetRequestCount_ConcurrentRequests_IncrementsCountCorrectly()
        {
            var controller = new RequestCounterController();
            int numTasks = 100;
            Task<IActionResult>[] tasks = new Task<IActionResult>[numTasks];

            for (int i = 0; i < numTasks; i++)
            {
                tasks[i] = Task.Run(() => controller.GetRequestCount());
            }

            await Task.WhenAll(tasks);

            //After all tasks are completed, get the request count again
            var finalResult = controller.GetRequestCount() as OkObjectResult;

            Assert.IsNotNull(finalResult);
            Assert.AreEqual(200, finalResult.StatusCode);
            Assert.AreEqual(numTasks + 1, finalResult.Value);
        }
    }
}