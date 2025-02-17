// Program.cs
using UserApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using UserApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// app.Run();

// Start the Web Application (ASP.NET Core API)
var serverTask = Task.Run(async () =>
{
    await app.RunAsync();  // Run the API server
});

// Assuming your API will be available at this URL after startup
string baseUrl = "http://localhost:5294/api/user/";

// Simulate calling the API after starting the application
await CallApiEndpoints(baseUrl);

await serverTask;

static async Task CallApiEndpoints(string baseUrl)
{
    using var client = new HttpClient();

    // Wait for the API to start before making requests
    await Task.Delay(5000);  // Wait for 5 seconds to ensure the server is up and running

    // Send GET request to GetUser endpoint ()
    HttpResponseMessage response = await client.GetAsync(baseUrl + "1");

    if (response.IsSuccessStatusCode)
    {
        // Read and display the response
        string content = await response.Content.ReadAsStringAsync();
        Console.WriteLine("Response from GetUser endpoint:");
        Console.WriteLine(content);
    }
    else
    {
        Console.WriteLine("Error: " + response.StatusCode);
    }

    // Send GET request to GetProfile endpoint (e.g., https://localhost:5001/api/user/profile)
    response = await client.GetAsync(baseUrl + "profile");

    if (response.IsSuccessStatusCode)
    {
        // Read and display the response
        string content = await response.Content.ReadAsStringAsync();
        Console.WriteLine("Response from GetProfile endpoint:");
        Console.WriteLine(content);
    }
    else
    {
        Console.WriteLine("Error: " + response.StatusCode);
    }
}
