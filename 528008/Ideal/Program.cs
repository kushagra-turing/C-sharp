using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.Timer;
using System.Threading.Tasks;

namespace DurableOrchestrationExample
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            // Create a new host builder to configure services and functions.
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()  // This sets up the function worker
                .ConfigureServices(services =>
                {
                    // You can add services here if needed, like adding a test logger, etc.
                })
                .Build();

            // Start the host, which will start the Azure Functions runtime and await execution.
            await host.RunAsync();
        }
    }
}
