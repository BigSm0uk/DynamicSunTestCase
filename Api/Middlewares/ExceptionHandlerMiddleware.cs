using System.Net;
using Newtonsoft.Json;

namespace Api.Middlewares;

public class ExceptionHandlerMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)    
    {    
        try    
        {    
            await next.Invoke(context);    
        }    
        catch (Exception ex)    
        {    
            await HandleExceptionMessageAsync(context, ex).ConfigureAwait(false);  
        }    
    }    
    private static Task HandleExceptionMessageAsync(HttpContext context, Exception exception)  
    {  
        context.Response.ContentType = "application/json";  
        const int statusCode = (int)HttpStatusCode.InternalServerError;  
        var result = JsonConvert.SerializeObject(new  
        {  
            StatusCode = statusCode,  
            ErrorMessage = exception.Message  
        });  
        context.Response.ContentType = "application/json";  
        context.Response.StatusCode = statusCode;  
        return context.Response.WriteAsync(result);  
    } 
}

public static class ExceptionHandlerMiddlewareExtensions  
{  
    public static void UseExceptionHandlerMiddleware(this IApplicationBuilder app)  
    {  
        app.UseMiddleware<ExceptionHandlerMiddleware>();  
    }  
} 