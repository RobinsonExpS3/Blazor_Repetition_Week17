using System.Diagnostics;

namespace BlazorRep.API.Middleware;

public class CustomLoggingMiddleware {
    private readonly RequestDelegate _next;
    private readonly ILogger<CustomLoggingMiddleware> _logger;

    public CustomLoggingMiddleware(RequestDelegate next, ILogger<CustomLoggingMiddleware> logger) {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context) {
        var stopwatch = Stopwatch.StartNew();

        await _next(context);

        stopwatch.Stop();

        _logger.LogInformation(
            "RequestPath: {RequestPath}, RequestMethod: {RequestMethod}, ResponseTidMs: {ResponseTimeMs}, ResponseStatuskode: {StatusCode}",
            context.Request.Path,
            context.Request.Method,
            stopwatch.ElapsedMilliseconds,
            context.Response.StatusCode);
    }
}
