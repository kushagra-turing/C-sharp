using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DurableOrchestrationExample
{
    public static class OrchestratorFunctions
    {
        [FunctionName("RunOrchestrator")]
        public static async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            Console.WriteLine($"Orchestration started: {context.InstanceId}");

            // Loop for 10 seconds
            for(int i = 0; i < 10; i++)
            {
                string resultHello = await context.CallActivityAsync<string>("Hello", null);
                string resultBye = await context.CallActivityAsync<string>("Bye", null);
                await context.CreateTimer(context.CurrentUtcDateTime.AddSeconds(1), CancellationToken.None);
            }
            Console.WriteLine($"Orchestration completed: {context.InstanceId}");
        }

        [FunctionName("Hello")]
        public static string Hello([ActivityTrigger] IDurableActivityContext context)
        {
            Console.WriteLine("Hello");
            return "Hello";
        }

        [FunctionName("Bye")]
        public static string Bye([ActivityTrigger] IDurableActivityContext context)
        {
            Console.WriteLine("Bye");
            return "Bye";
        }

        [FunctionName("TimerStarter")]
        public static async Task TimerStarter(
            [TimerTrigger("0 */1 * * * *")] TimerInfo timerInfo, // Every minute. Adjust as needed for testing.
            [DurableClient] IDurableOrchestrationClient starter)
        {
            string instanceId = Guid.NewGuid().ToString();
            Console.WriteLine($"Starting orchestration with ID = '{instanceId}'.");
            await starter.StartNewAsync("RunOrchestrator", instanceId);
        }

        public static class ClientFunctions
        {
            [FunctionName("StartOrchestration")]
            public static async Task<IActionResult> StartOrchestration(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "orchestrator/{functionName}")] HttpRequest req,
            [DurableClient] IDurableOrchestrationClient durableClient,
            string functionName)
            {
                string instanceId = Guid.NewGuid().ToString();
                await durableClient.StartNewAsync(functionName, instanceId);

                return new OkObjectResult(instanceId);
            }

            [FunctionName("RaiseEvent")]
            public static async Task<IActionResult> RaiseEvent(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "orchestrator/{instanceId}/raiseEvent/{eventName}")] HttpRequest req,
            [DurableClient] IDurableOrchestrationClient durableClient,
            string instanceId, string eventName)
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                await durableClient.RaiseEventAsync(instanceId, eventName, requestBody);

                return new OkResult();
            }

            [FunctionName("TerminateOrchestration")]
            public static async Task<IActionResult> TerminateOrchestration(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "orchestrator/{instanceId}/terminate")] HttpRequest req,
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
