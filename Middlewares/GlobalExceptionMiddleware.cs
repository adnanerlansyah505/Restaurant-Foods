using System.Net;
using System.Text.Json;
using RestaurantFoods.Exceptions;
using RestaurantFoods.Utilities.Handlers;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (JsonException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            var response = new ResponseHandlers<object>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Invalid JSON format. Please check your request body."
            };

            await context.Response.WriteAsJsonAsync(response);
        }
        catch (AppException ex)
        {
            context.Response.StatusCode = ex.StatusCode;
            context.Response.ContentType = "application/json";

            var response = new ResponseHandlers<object>
            {
                Code = ex.StatusCode,
                Status = ((HttpStatusCode)ex.StatusCode).ToString(),
                Message = ex.Message,
                Errors = new Dictionary<string, List<string>>
                {
                    { "errorCode", new List<string> { ex.ErrorCode }}
                }
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            Console.WriteLine(e.Message);

            var response = new ResponseHandlers<object>
            {
                Code = 500,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Internal server error"
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}