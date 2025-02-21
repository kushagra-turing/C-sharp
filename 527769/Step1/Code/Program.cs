using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using SmartFormat;
using SmartFormat.Core.Settings;
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

            // Test the endpoint (simulate a GET request)
            using (var client = new HttpClient { BaseAddress = new Uri("http://localhost:5000") })
            {
                var response = await client.GetAsync("/User/details");
                var responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response from API: " + responseString);
            }

            // Allow the app to run for a while to process the request
            await Task.Delay(5000); // Delay added to ensure the server can process the request before shutting down
        }
    }
}