using System.Diagnostics;

namespace WebApi.Observability;

/// <summary>
/// Adds a correlation identifier to each request and records an operational
/// request summary. It deliberately does not log request bodies or headers so
/// that sensitive data is not unnecessarily captured in application logs.
/// </summary>
public sealed class RequestLoggingMiddleware
{
    private const string CorrelationHeader = "X-Correlation-ID";

    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(
        RequestDelegate next,
        ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = GetOrCreateCorrelationId(context);
        context.TraceIdentifier = correlationId;
        context.Response.Headers[CorrelationHeader] = correlationId;

        var stopwatch = Stopwatch.StartNew();

        try
        {
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();

            _logger.LogInformation(
                "HTTP {Method} {Path} returned {StatusCode} in {ElapsedMs} ms. CorrelationId={CorrelationId}",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds,
                correlationId);
        }
    }

    private static string GetOrCreateCorrelationId(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue(CorrelationHeader, out var supplied)
            && Guid.TryParse(supplied.FirstOrDefault(), out var parsed))
        {
            return parsed.ToString();
        }

        return Guid.NewGuid().ToString();
    }
}
