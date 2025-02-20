using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace RequestCounterApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Start the Web API server
            var host = CreateHostBuilder(args).Build();

            // Start the server in a separate thread
            var serverThread = new Thread(() => host.RunAsync());
            serverThread.Start();

            // Wait until the server is up before sending requests
            WaitForServerToBeReady();

            // Simulate concurrent requests to trigger the race condition
            SimulateConcurrentRequests();

            // Allow some time for the requests to complete
            Thread.Sleep(1000);  // Sleep for 1 seconds

            // Stop the server after simulation
            host.StopAsync().Wait();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureServices(services =>
                    {
                        // Register services needed by the application
                        services.AddControllers();
                        services.AddEndpointsApiExplorer();
                        services.AddSwaggerGen();
                    });

                    webBuilder.Configure(app =>
                    {
                        // Configure middleware
                        app.UseRouting();
                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapControllers();  // Map controllers
                        });

                        // Add Swagger UI for API documentation
                        app.UseSwagger();
                        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RequestCounter API v1"));
                    });
                });

        private static void WaitForServerToBeReady()
        {
            // Simple retry mechanism to check if the server is accepting requests
            var httpClient = new HttpClient();
            var retries = 0;

            while (retries < 10)
            {
                try
                {
                    var response = httpClient.GetAsync("http://localhost:5115/RequestCounter/count").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Server is ready.");
                        return;
                    }
                }
                catch (Exception)
                {
                    // Ignore exceptions and retry
                }

                retries++;
                Thread.Sleep(1000);  // Retry after 1 second
            }

            Console.WriteLine("Server is not ready after multiple attempts.");
        }

        private static void SimulateConcurrentRequests()
        {
            var httpClient = new HttpClient();
            var tasks = new Task[1000];  // Simulate 100 concurrent requests
            var count = "";

            for (int i = 0; i < 1000; i++)
            {
                tasks[i] = Task.Run(async () =>
                {
                    // Call the RequestCounterController API endpoint
                    var response = await httpClient.GetAsync("http://localhost:5115/RequestCounter/count");
                    if (response.IsSuccessStatusCode)
                    {
                        count = await response.Content.ReadAsStringAsync();
                        // Console.WriteLine($"Request Count: {count}");
                    }
                });
            }

            // Wait for all tasks to complete
            Task.WhenAll(tasks).Wait();

            Console.WriteLine($"Request Count: {count}");
        }
    }
}
