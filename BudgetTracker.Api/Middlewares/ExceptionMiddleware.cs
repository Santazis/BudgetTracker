using System.Text.Json;
using BudgetTracker.Domain.Common.Exceptions;

namespace BudgetTracker.Middlewares;

public class ExceptionMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
           ErrorDetails errorDetails = new ErrorDetails()
           {
               Message = e.Message
           };
           HttpResponse httpResponse = context.Response;
           if(!httpResponse.HasStarted)
            {
                httpResponse.StatusCode = 500;
                httpResponse.ContentType = "application/json";
                JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                await context.Response.WriteAsJsonAsync(errorDetails, jsonSerializerOptions);
            }
        }
    }
}