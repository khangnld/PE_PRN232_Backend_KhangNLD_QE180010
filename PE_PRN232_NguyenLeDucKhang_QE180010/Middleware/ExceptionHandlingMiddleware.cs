using System.Net;
using System.Text.Json;
using PE_PRN232_NguyenLeDucKhang_QE180010.DTOs.Responses;

namespace PE_PRN232_NguyenLeDucKhang_QE180010.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        var response = new ApiResponse<object>
        {
            Success = false,
            Message = "An error occurred while processing your request",
            Errors = new List<string>()
        };

        switch (exception)
        {
            case ArgumentException argEx:
                response.Message = argEx.Message;
                response.Errors.Add(argEx.Message);
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;
            case KeyNotFoundException:
                response.Message = "Resource not found";
                response.Errors.Add("Resource not found");
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                break;
            default:
                response.Message = "An internal server error occurred";
                response.Errors.Add(exception.Message);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }

        var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        return context.Response.WriteAsync(jsonResponse);
    }
}



