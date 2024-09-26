using System;
using System.Net;
using System.Text.Json;
using banking.Errors;

namespace banking.Middleware;

// Handles and serializes exceptions into JSON response format
public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
{
    private readonly RequestDelegate _next = next; // The next middleware in the pipeline
    private readonly ILogger<ExceptionMiddleware> _logger = logger; // Logger instance for logging exceptions
    private readonly IHostEnvironment _env = env; // Environment information (Development or Production)
    public async Task InvokeAsync(HttpContext context)
    {
        try 
        {
            await _next(context); // Call the next middleware in the pipeline
        }
        catch (Exception ex) // Catch any thrown exception
        {
            _logger.LogError(ex, ex.Message); // Log the exception details

            context.Response.ContentType = "application/json"; // Set response content type to JSON
            // Set response status code to 500 Internal Server Error
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; 

            var response = _env.IsDevelopment() ?
                 // In development mode, include stack trace in the response
                new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString()) : 
                // In production mode, display a generic error message
                new ApiException(context.Response.StatusCode, ex.Message, "Internal Server Error"); 

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase // Use camel case for JSON property names
            };
            var json = JsonSerializer.Serialize(response, options); // Serialize the ApiException instance to JSON format
            await context.Response.WriteAsync(json); // Write the serialized JSON response back to the client
        }
    }
}
