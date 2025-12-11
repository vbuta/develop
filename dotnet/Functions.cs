using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Runtime.InteropServices;
using System;

public class Functions
{
    private readonly ILogger _logger;

    public Functions(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<Functions>();
    }

    // --- HELLO WORLD ---
    [Function("HelloWorld")]
    public HttpResponseData HelloWorld(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "hello_world")] HttpRequestData req)
    {
        _logger.LogInformation("Processing HelloWorld request");
        var response = req.CreateResponse();
        response.WriteString("Hello World !");
        return response;
    }

    // --- LOG ---
    [Function("Log")]
    public HttpResponseData Log(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "log")] HttpRequestData req)
    {
        _logger.LogInformation("Log endpoint called");
        _logger.LogInformation("Console out message");
        _logger.LogError("Console error message");

        var response = req.CreateResponse();
        response.WriteString("Logged!");
        return response;
    }

    // --- VERSION ---
    [Function("Version")]
    public HttpResponseData Version(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "version")] HttpRequestData req)
    {
        var versionInfo = new
        {
            dotnet_runtime = Environment.Version.ToString(),
            os = RuntimeInformation.OSDescription,
            framework = RuntimeInformation.FrameworkDescription
        };

        var response = req.CreateResponse();
        response.Headers.Add("Content-Type", "application/json");
        response.WriteString(JsonSerializer.Serialize(versionInfo, new JsonSerializerOptions { WriteIndented = true }));

        return response;
    }

    // --- STORAGE ACCOUNT INFO ---
    [Function("SAInfo")]
    public HttpResponseData SAInfo(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "sa_info")] HttpRequestData req)
    {
        string? storageName = Environment.GetEnvironmentVariable("AzureWebJobsStorage__accountName")
                              ?? "Unknown";

        var message = new
        {
            StorageAccountName = storageName
        };

        var response = req.CreateResponse();
        response.Headers.Add("Content-Type", "application/json");
        response.WriteString(JsonSerializer.Serialize(message, new JsonSerializerOptions { WriteIndented = true }));

        return response;
    }
}

