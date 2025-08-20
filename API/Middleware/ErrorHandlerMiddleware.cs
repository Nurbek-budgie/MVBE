using System.Net;
using System.Text.Json;
using DTO.Middleware;

namespace API.Middleware;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            var responseApi = ApiResponse<string>.Failure(ex.Message);
            switch (ex)
            {
                default:
                    response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    break;
            }
            var result = JsonSerializer.Serialize(responseApi);
            await response.WriteAsync(result);
        }
    }
}