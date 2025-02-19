public class LogFilterMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LogFilterMiddleware> _logger;

    private static string[] _excludablePaths = ["/healthz"];

    public LogFilterMiddleware(RequestDelegate next, ILogger<LogFilterMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Check if the request path should be logged
        if (!context.Request.Path.StartsWithSegments("/api/specific-path") &&
            !context.Request.Path.StartsWithSegments("/another-path"))
        {
            _logger.LogInformation($"Request: {context.Request.Method} {context.Request.Path}");
        }

        await _next(context);
    }
}
