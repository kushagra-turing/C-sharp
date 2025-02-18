using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace DurableOrchestrationExample
{
    public static class OrchestratorFunctions
    {
        [FunctionName("RunOrchestrator")]
        public static async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            Console.WriteLine($"Orchestration started: {context.InstanceId}");

            string resultHello = await context.CallActivityAsync<string>("Hello");
            Console.WriteLine(resultHello);

            string resultBye = await context.CallActivityAsync<string>("Bye");
            Console.WriteLine(resultBye);

            Console.WriteLine($"Orchestration completed: {context.InstanceId}");
        }

        [FunctionName("Hello")]
        public static string Hello([ActivityTrigger] IDurableActivityContext context)
        {
            Console.WriteLine("Executing Hello activity.");
            return "Hello from activity!";
        }

        [FunctionName("Bye")]
        public static string Bye([ActivityTrigger] IDurableActivityContext context)
        {
            Console.WriteLine("Executing Bye activity.");
            return "Bye from activity!";
        }

        [FunctionName("TimerStarter")]
        public static async Task TimerStarter(
            [TimerTrigger("0 */1 * * * *")] TimerInfo timerInfo, // Every minute.  Adjust as needed for testing.
            [DurableClient] IDurableOrchestrationClient starter)
        {
            string instanceId = Guid.NewGuid().ToString();
            Console.WriteLine($"Starting orchestration with ID = '{instanceId}'.");
            await starter.StartNewAsync("RunOrchestrator", instanceId);
        }

        [FunctionName("ClientFunctions")]
        public static class ClientFunctions
        {
            [FunctionName("StartOrchestration")]
            public static async Task<IActionResult> StartOrchestration(
            [HttpTrigger(Microsoft.AspNetCore.Http.Extensions.HttpMethod.Get, Route = "orchestrator/{functionName}")] HttpRequest req,
            [DurableClient] IDurableOrchestrationClient durableClient,
            string functionName)
            {
                string instanceId = Guid.NewGuid().ToString();
                await durableClient.StartNewAsync(functionName, instanceId);

                return new OkObjectResult(instanceId);
            }

            [FunctionName("RaiseEvent")]
            public static async Task<IActionResult> RaiseEvent(
            [HttpTrigger(Microsoft.AspNetCore.Http.Extensions.HttpMethod.Post, Route = "orchestrator/{instanceId}/raiseEvent/{eventName}")] HttpRequest req,
            [DurableClient] IDurableOrchestrationClient durableClient,
            string instanceId, string eventName)
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                await durableClient.RaiseEventAsync(instanceId, eventName, requestBody);

                return new OkResult();
            }

            [FunctionName("TerminateOrchestration")]
            public static async Task<IActionResult> TerminateOrchestration(
            [HttpTrigger(Microsoft.AspNetCore.Http.Extensions.HttpMethod.Post, Route = "orchestrator/{instanceId}/terminate")] HttpRequest req,
            [DurableClient] IDurableOrchestrationClient durableClient,
            string instanceId)
            {
                string reason = await new StreamReader(req.Body).ReadToEndAsync();
                await durableClient.TerminateAsync(instanceId, reason);

                return new OkResult();
            }
        }
    }
}