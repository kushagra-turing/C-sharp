using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;
using SmartFormat;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace YourProjectName
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Set up the in-memory test server for the API
            var builder = WebApplication.CreateBuilder(args);

            // Register services (including controllers)
            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure middleware and endpoints
            app.UseRouting();
            app.MapControllers();

            // Start the app in the background
            Task.Run(() => app.RunAsync());

            // Wait a few seconds to ensure the server is up and running
            Console.WriteLine("Waiting for server to start...");
            await Task.Delay(2000);  // Adjust this delay as needed
            
            // Test the endpoint (simulate a GET request)
            using (var client = new HttpClient { BaseAddress = new Uri("http://localhost:5115") })
            {
                try
                {
                    var response = await client.GetAsync("/User/details");
                    
                    // Print the HTTP status code
                    Console.WriteLine($"HTTP Response Status Code: {response.StatusCode}");

                    // Read and print the response content
                    var responseString = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Response from API: " + responseString);
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

            // Allow the app to run for a while to process the request before shutting down
            await Task.Delay(5000); // Delay added to ensure the server can process the request before shutting down
        }
    }
}
