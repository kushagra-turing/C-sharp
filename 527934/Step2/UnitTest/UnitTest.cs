// Code: NUnit Tests (RequestCounterControllerTests.cs)
using NUnit.Framework;
using RequestCounterApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace RequestCounterApiTests
{
    public class RequestCounterControllerTests
    {
        [SetUp]
        public void Setup()
        {
            // Reset the counter before each test (ideally using reflection or a dedicated reset method
            // in the controller, but for simplicity, a direct assignment here. Be mindful of thread safety
            // if Reset is called in parallel)
            System.Reflection.FieldInfo field = typeof(RequestCounterController).GetField("_requestCount", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            if (field != null)
            {
                field.SetValue(null, 0);
            }
        }

        [Test]
        public void GetRequestCount_IncrementsCounter()
        {
            var controller = new RequestCounterController();

            var result1 = controller.GetRequestCount() as OkObjectResult;
            Assert.AreEqual(1, result1.Value);

            var result2 = controller.GetRequestCount() as OkObjectResult;
            Assert.AreEqual(2, result2.Value);
        }

        [Test]
        public async Task GetRequestCount_ConcurrentRequests()
        {
            var controller = new RequestCounterController();
            int numTasks = 10;
            Task<IActionResult>[] tasks = new Task<IActionResult>[numTasks];

            for (int i = 0; i < numTasks; i++)
            {
                tasks[i] = Task.Run(() => controller.GetRequestCount());
            }

            await Task.WhenAll(tasks);

            // After all tasks complete, the counter should be equal to the number of tasks.
            var finalResult = controller.GetRequestCount() as OkObjectResult;
            Assert.AreEqual(numTasks + 1, finalResult.Value); // +1 because we call GetRequestCount again after all tasks are done.
        }
    }
}