using Microsoft.AspNetCore.Mvc;

public class ProblemDetailsMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ProblemDetailsMiddleware> _logger;

    public ProblemDetailsMiddleware(RequestDelegate next, ILogger<ProblemDetailsMiddleware> logger)
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
            _logger.LogError(ex, "An unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = StatusCodes.Status500InternalServerError;
        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = "An unexpected error occurred",
            Detail = exception.Message,
            Instance = context.Request.Path
        };

        if (exception is ArgumentNullException)
        {
            problemDetails.Status = StatusCodes.Status400BadRequest;
            problemDetails.Title = "A required parameter is missing.";
        }
        else if (exception is UnauthorizedAccessException)
        {
            problemDetails.Status = StatusCodes.Status401Unauthorized;
            problemDetails.Title = "Unauthorized access.";
        }

        context.Response.StatusCode = problemDetails.Status.Value;
        context.Response.ContentType = "application/problem+json";
        return context.Response.WriteAsJsonAsync(problemDetails);
    }
}
