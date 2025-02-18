using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using System.Threading;

namespace DurableTaskExample
{
    public static class OrchestratorExample
    {
        [FunctionName("HelloActivity")]
        public static string Hello([ActivityTrigger] string name)
        {
            Console.WriteLine($"Hello, {name}!");
            return $"Hello, {name}!";
        }

        [FunctionName("ByeActivity")]
        public static string Bye([ActivityTrigger] string name)
        {
            Console.WriteLine($"Bye, {name}!");
            return $"Bye, {name}!";
        }

        [FunctionName("OrchestratorFunction")]
        public static async Task RunOrchestrator([OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            Console.WriteLine("Orchestrator started.");
            await context.CallActivityAsync<string>("HelloActivity", "World");
            await context.CallActivityAsync<string>("ByeActivity", "World");
            Console.WriteLine("Orchestrator finished.");
        }

        [FunctionName("OrchestratorStarter")]
        public static async Task<string> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            // Generate an instance id
            string instanceId = Guid.NewGuid().ToString();
            // Function input comes from the request content.
            object eventData = null;
            await starter.StartNewAsync("OrchestratorFunction", instanceId, eventData);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId).ToString();
        }
        public static async Task RunClient(IDurableOrchestrationClient client, string instanceId)
        {
             await client.StartNewAsync("OrchestratorFunction", instanceId);
             Console.WriteLine($"Started orchestration with ID = '{instanceId}'.");
        }
    }

    //Mock Durable Client for Unit Testing
    public class MockDurableOrchestrationClient : IDurableOrchestrationClient
    {
        public int CallActivityAsyncCallCount { get; set; } = 0;
        public int StartNewAsyncCallCount { get; set; } = 0;

        public Task<string> StartNewAsync(string orchestratorFunctionName, object input)
        {
            StartNewAsyncCallCount++;
            Console.WriteLine($"MockDurableOrchestrationClient: StartNewAsync called with {orchestratorFunctionName}");
            return Task.FromResult(Guid.NewGuid().ToString()); // Return a dummy instance ID.
        }

        public Task<string> StartNewAsync(string orchestratorFunctionName, string instanceId, object input)
        {
            StartNewAsyncCallCount++;
            Console.WriteLine($"MockDurableOrchestrationClient: StartNewAsync called with {orchestratorFunctionName} and instanceId {instanceId}");
            return Task.FromResult(instanceId); // Return the provided instance ID
        }
        public Task<string> StartNewAsync(string orchestratorFunctionName, string instanceId, object input, DateTimeOffset? startTime)
        {
            StartNewAsyncCallCount++;
            Console.WriteLine($"MockDurableOrchestrationClient: StartNewAsync called with {orchestratorFunctionName} and instanceId {instanceId}");
            return Task.FromResult(instanceId); // Return the provided instance ID
        }
        public Task<string> StartNewAsync(string orchestratorFunctionName, string instanceId, object input, TimeSpan timeout)
        {
            StartNewAsyncCallCount++;
            Console.WriteLine($"MockDurableOrchestrationClient: StartNewAsync called with {orchestratorFunctionName} and instanceId {instanceId}");
            return Task.FromResult(instanceId); // Return the provided instance ID
        }

        public Task<T> CallActivityAsync<T>(string activityFunctionName, object input)
        {
            CallActivityAsyncCallCount++;
            Console.WriteLine($"MockDurableOrchestrationClient: CallActivityAsync called with {activityFunctionName}");
            return Task.FromResult((T)Convert.ChangeType($"Result from {activityFunctionName}", typeof(T))); // Return a dummy result.
        }

        public Task CallActivityAsync(string activityFunctionName, object input)
        {
            CallActivityAsyncCallCount++;
            Console.WriteLine($"MockDurableOrchestrationClient: CallActivityAsync called with {activityFunctionName}");
            return Task.CompletedTask;
        }

         public Task<T> CallActivityAsync<T>(string activityFunctionName, string input)
        {
            CallActivityAsyncCallCount++;
            Console.WriteLine($"MockDurableOrchestrationClient: CallActivityAsync called with {activityFunctionName}");
            return Task.FromResult((T)Convert.ChangeType($"Result from {activityFunctionName}", typeof(T))); // Return a dummy result.
        }


        public Task<DurableOrchestrationStatus> GetStatusAsync(string instanceId, bool showHistory, bool showHistoryOutput, bool showInput)
        {
            throw new NotImplementedException();
        }

        public Task<IList<DurableOrchestrationStatus>> GetStatusAsync(DateTime createdTimeFrom, DateTime? createdTimeTo, IEnumerable<OrchestrationStatus> runtimeStatus, int top, string continuationToken)
        {
            throw new NotImplementedException();
        }

        public Task<PurgeHistoryResult> PurgeInstanceHistoryAsync(string instanceId)
        {
            throw new NotImplementedException();
        }

        public Task<PurgeHistoryResult> PurgeInstanceHistoryAsync(DateTime createdTimeFrom, DateTime? createdTimeTo, IEnumerable<OrchestrationStatus> runtimeStatus)
        {
            throw new NotImplementedException();
        }

        public Task RaiseEventAsync(string instanceId, string eventName, object eventData)
        {
            throw new NotImplementedException();
        }

        public Task RewindAsync(string instanceId, string reason)
        {
            throw new NotImplementedException();
        }

        public Task TerminateAsync(string instanceId, string reason)
        {
            throw new NotImplementedException();
        }
        public string CreateHttpManagementPayload(string instanceId)
        {
            throw new NotImplementedException();
        }

        public HttpManagementPayload CreateHttpManagementPayload(string instanceId, OrchestrationRuntimeStatus[] status)
        {
            throw new NotImplementedException();
        }

        public Task<DurableOrchestrationStatusQueryResult> ListInstancesAsync(OrchestrationStatusQueryCondition condition, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<DurableOrchestrationStatusQueryResult> IDurableOrchestrationClient.ListInstancesAsync(OrchestrationStatusQueryCondition condition)
        {
            throw new NotImplementedException();
        }

        public Task CleanHistoryAsync(int retentionDays)
        {
            throw new NotImplementedException();
        }

        public Task CleanHistoryAsync(DateTime thresholdDateTimeUtc, OrchestrationRuntimeStatus[] timeRangeFilterType)
        {
            throw new NotImplementedException();
        }

         public string CreateCheckStatusResponse(HttpRequest req, string instanceId)
        {
            throw new NotImplementedException();
        }
    }
    // Dummy ILogger interface for compilation purpose.
    public interface ILogger
    {
         void LogInformation(string message);
    }

    public class DummyLogger : ILogger
    {
        public void LogInformation(string message)
        {
            Console.WriteLine(message);
        }
    }

    public class HttpRequest
    {
       public object Body { get; set; }
    }

    public enum AuthorizationLevel
    {
        Anonymous
    }
}