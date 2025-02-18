using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DurableTaskOrchestrator
{
    public static class OrchestratorFunctions
    {
        [FunctionName("RunOrchestrator")]
        public static async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            Console.WriteLine("Orchestrator started."); // Use Console.WriteLine for logging

            string resultHello = await context.CallActivityAsync<string>("Hello");
            Console.WriteLine(resultHello);

            string resultBye = await context.CallActivityAsync<string>("Bye");
            Console.WriteLine(resultBye);

            Console.WriteLine("Orchestrator finished."); // Use Console.WriteLine for logging
        }

        [FunctionName("Hello")]
        public static string Hello([ActivityTrigger] IDurableActivityContext context)
        {
            return "Hello!";
        }

        [FunctionName("Bye")]
        public static string Bye([ActivityTrigger] IDurableActivityContext context)
        {
            return "Bye!";
        }

        [FunctionName("TimerStarter")]
        public static async Task TimerStarter(
            [TimerTrigger("0 */1 * * * *")] // Every minute (for initial testing, change to appropriate interval)
            TimerInfo timerInfo,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log) //Note: Logger is needed here for the initial function trigger
        {
            string instanceId = Guid.NewGuid().ToString();
            await starter.StartNewAsync("RunOrchestrator", instanceId);
            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");
        }
    }

    // Helper class for testing purposes to simulate function execution
    public class TestDurableOrchestrationClient : IDurableOrchestrationClient
    {
        public List<string> Output { get; set; } = new List<string>();
        public string LastInstanceId { get; set; }

        public Task<string> StartNewAsync(string functionName, object input = null)
        {
            LastInstanceId = Guid.NewGuid().ToString();
            Output.Add($"Started orchestration: {functionName} with instance ID: {LastInstanceId}");
            return Task.FromResult(LastInstanceId);
        }

        public Task<string> StartNewAsync(string functionName, string instanceId, object input = null)
        {
            LastInstanceId = instanceId;
            Output.Add($"Started orchestration: {functionName} with instance ID: {LastInstanceId}");
            return Task.FromResult(instanceId);
        }


        public Task RaiseEventAsync(string instanceId, string eventName, object eventData)
        {
            Output.Add($"Raised event: {eventName} for instance ID: {instanceId}");
            return Task.CompletedTask;
        }

        public Task<DurableOrchestrationStatus> GetStatusAsync(string instanceId)
        {
            // Mock status retrieval
            return Task.FromResult(new DurableOrchestrationStatus
            {
                InstanceId = instanceId,
                RuntimeStatus = OrchestrationRuntimeStatus.Running
            });
        }

        public Task<PurgeHistoryResult> PurgeInstanceHistoryAsync(string instanceId)
        {
            Output.Add($"Purged history for instance ID: {instanceId}");
            return Task.FromResult(new PurgeHistoryResult(1));
        }

        public Task TerminateAsync(string instanceId, string reason)
        {
            Output.Add($"Terminated instance ID: {instanceId} with reason: {reason}");
            return Task.CompletedTask;
        }

        public Task<OrchestrationStatusQueryResult> ListInstancesAsync(OrchestrationStatusQueryCondition condition, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<OrchestrationStatusQueryResult> ListInstancesAsync(DateTime createdTimeFrom, DateTime? createdTimeTo, IEnumerable<OrchestrationRuntimeStatus> runtimeStatus, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RewindAsync(string instanceId, string reason)
        {
            throw new NotImplementedException();
        }

        #region Unimplemented Interface Members
        public Task<DurableOrchestrationStatus> WaitForCompletionOrCreateCheckStatusResponseAsync(HttpRequestMessage request, string instanceId, TimeSpan? timeout = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> CreateCheckStatusResponseAsync(HttpRequestMessage request, string instanceId)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> CreateHttpManagementPayloadAsync(string instanceId)
        {
            throw new NotImplementedException();
        }

        public Task<DurableOrchestrationStatus?> GetStatusAsync(string instanceId, bool showInput)
        {
            throw new NotImplementedException();
        }

        public Task<IList<DurableOrchestrationStatus>> GetStatusAsync(IEnumerable<string> instanceId)
        {
            throw new NotImplementedException();
        }

        public Task PurgeInstanceHistoryAsync(DateTime createdTimeFrom, DateTime? createdTimeTo, IEnumerable<OrchestrationStatus> runtimeStatus)
        {
            throw new NotImplementedException();
        }

        public Task<PurgeHistoryResult> PurgeInstanceHistoryAsync(OrchestrationStatusQueryCondition condition)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}