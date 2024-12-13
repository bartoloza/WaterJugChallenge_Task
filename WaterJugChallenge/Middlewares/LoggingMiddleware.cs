public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var traceId = context.TraceIdentifier;

        _logger.LogInformation($"Incoming request: {context.Request.Method} {context.Request.Path} - TraceId: {traceId}");

        try
        {
            await _next(context); // Proceed to the next middleware
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while processing the request: {TraceId}", traceId);

            // Do not return the error response in this middleware; 
            // it will be handled by ErrorHandlerMiddleware.
            throw;  // Re-throw the exception for the error handler to process
        }

        _logger.LogInformation($"Outgoing response: {context.Response.StatusCode} - TraceId: {traceId}");
    }
}
