using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Cqrs.Template.Infra.CrossCutting.IoC.Configurations.Logging;

internal class HttpRequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<HttpRequestResponseLoggingMiddleware> _logger;

    public HttpRequestResponseLoggingMiddleware(RequestDelegate next, ILogger<HttpRequestResponseLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        var requestId = Guid.NewGuid();
        const string requestResponseLoggingTemplate = "HTTP request information ({RequestId}):" +
                                                      "\nMethod: {Method}" +
                                                      "\nPath: {Path}" +
                                                      "\nQueryString: {QueryString}" +
                                                      "\nHeaders: {Headers}" +
                                                      "\nSchema: {Schema}" +
                                                      "\nHost: {Host}" +
                                                      "\nBody: {Body}";

        _logger.LogInformation(
            requestResponseLoggingTemplate,
            requestId,
            httpContext.Request.Method,
            httpContext.Request.Path,
            httpContext.Request.QueryString,
            httpContext.Request.Headers,
            httpContext.Request.Scheme,
            httpContext.Request.Host,
            await ReadBodyFromRequest(httpContext.Request)
        );

        // Temporarily replace the HttpResponseStream, which is a write-only stream, with a MemoryStream to capture it's value in-flight.
        var originalResponseBody = httpContext.Response.Body;

        await using var newResponseBody = new MemoryStream();
        httpContext.Response.Body = newResponseBody;

        // Call the next middleware in the pipeline
        await _next(httpContext);

        newResponseBody.Seek(0, SeekOrigin.Begin);
        var responseBodyText = await new StreamReader(httpContext.Response.Body).ReadToEndAsync();


        const string httpResponseTemplate = "HTTP response information ({RequestId}):" +
                                            "\nStatusCode: {StatusCode}" +
                                            "\nContentType: {ContentType}" +
                                            "\nHeaders: {Headers}" +
                                            "\nBody: {Body}";

        _logger.LogInformation(
            httpResponseTemplate,
            requestId,
            httpContext.Response.StatusCode,
            httpContext.Response.ContentType,
            FormatHeaders(httpContext.Response.Headers),
            responseBodyText
        );

        newResponseBody.Seek(0, SeekOrigin.Begin);
        await newResponseBody.CopyToAsync(originalResponseBody);
    }

    private static string FormatHeaders(IHeaderDictionary headers) => string.Join(", ", headers.Select(kvp => $"{{{kvp.Key}: {string.Join(", ", kvp.Value)}}}"));

    private static async Task<string> ReadBodyFromRequest(HttpRequest request)
    {
        // Ensure the request's body can be read multiple times (for the next middlewares in the pipeline).
        request.EnableBuffering();

        using var streamReader = new StreamReader(request.Body, leaveOpen: true);
        var requestBody = await streamReader.ReadToEndAsync();

        // Reset the request's body stream position for next middleware in the pipeline.
        request.Body.Position = 0;
        return requestBody;
    }
}
