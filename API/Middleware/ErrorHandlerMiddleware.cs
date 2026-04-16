using System.Net;
using System.Text.Json;
using DTO.Middleware;

namespace API.Middleware;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    public ErrorHandlerMiddleware(
        RequestDelegate next,
        ILogger<ErrorHandlerMiddleware> logger,
        IHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception for {Method} {Path}",
                context.Request.Method, context.Request.Path);

            var (status, safeMessage) = ex switch
            {
                KeyNotFoundException      => (HttpStatusCode.NotFound,       ex.Message),
                UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Unauthorized"),
                ArgumentException         => (HttpStatusCode.BadRequest,     ex.Message),
                InvalidOperationException => (HttpStatusCode.Conflict,       ex.Message),
                _                         => (HttpStatusCode.InternalServerError,
                                              _environment.IsDevelopment()
                                                  ? ex.Message
                                                  : "An unexpected error occurred.")
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;
            await context.Response.WriteAsync(JsonSerializer.Serialize(ApiResponse<string>.Failure(safeMessage)));
        }
    }
}
