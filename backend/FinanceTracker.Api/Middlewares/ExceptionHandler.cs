using Microsoft.AspNetCore.Diagnostics;

namespace FinanceTracker.Api.Middlewares;

public static class ExceptionHandler
{
  public static void UseGlobalExceptionHandler(this WebApplication app)
  {
    app.UseExceptionHandler(exceptionHandlerApp =>
      exceptionHandlerApp.Run(async context =>
      {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        var (statusCode, message) = exception switch
        {
          KeyNotFoundException => (400, exception.Message),
          InvalidOperationException => (400, exception.Message),
          _ => (500, "An unexpected error occurred.")
        };

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(new { status = statusCode, error = message });
      }));
  }
}