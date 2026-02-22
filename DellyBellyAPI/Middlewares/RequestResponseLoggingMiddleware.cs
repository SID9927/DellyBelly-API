using DellyBelly.Domain.Entities;
using DellyBelly.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using DellyBelly.Shared.Helpers;
using System.Threading.Tasks;

namespace DellyBelly.API.Middlewares
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _scopeFactory;

        // Truncate response/request bodies to avoid inserting huge payloads into the DB.
        // Adjust these limits as needed.
        private const int MaxBodyLength = 4000;

        public RequestResponseLoggingMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
        {
            _next = next;
            _scopeFactory = scopeFactory;
        }

        public async Task Invoke(HttpContext context)
        {
            // Ensure the request body can be read multiple times
            context.Request.EnableBuffering();

            var requestTime = TrimToMilliseconds(DateTimeHelper.GetIndianTime());
            var requestBody = await ReadRequestBody(context.Request);
            var stopwatch = Stopwatch.StartNew();

            // Capture Response Data
            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            try
            {
                await _next(context);
            }
            finally
            {
                stopwatch.Stop();
                var duration = stopwatch.Elapsed;

                var responseContent = await ReadResponseBody(responseBody);

                // Restore the original response stream so the client gets the response
                await responseBody.CopyToAsync(originalBodyStream);

                var responseTime = TrimToMilliseconds(DateTimeHelper.GetIndianTime());
                var statusCode = context.Response.StatusCode;
                var method = context.Request.Method;
                var path = context.Request.Path.ToString();
                var queryString = context.Request.QueryString.HasValue
                    ? context.Request.QueryString.Value ?? string.Empty
                    : string.Empty;
                var ipAddress = context.Connection.RemoteIpAddress?.ToString();
                var durationStr = FormatDuration(duration);

                // Truncate large bodies so we never time out on INSERT
                var truncatedRequest = Truncate(requestBody ?? string.Empty, MaxBodyLength);
                var truncatedResponse = Truncate(responseContent, MaxBodyLength);

                // Fire-and-forget: log asynchronously so it NEVER blocks the HTTP response.
                // Uses a fresh DI scope + DbContext to avoid conflicts with the request's context.
                _ = Task.Run(async () =>
                {
                    try
                    {
                        using var scope = _scopeFactory.CreateScope();
                        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                        var apiLog = new ApiLog
                        {
                            RequestTime = requestTime,
                            ResponseTime = responseTime,
                            Duration = durationStr,
                            Method = method,
                            Path = path,
                            QueryString = queryString,
                            RequestBody = truncatedRequest,
                            ResponseBody = truncatedResponse,
                            StatusCode = statusCode,
                            IpAddress = ipAddress
                        };

                        db.ApiLogs.Add(apiLog);
                        await db.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        // Logging must NEVER crash the application.
                        // In production you'd forward this to a file/console logger.
                        Console.Error.WriteLine($"[ApiLog] Failed to write log: {ex.Message}");
                    }
                });
            }
        }

        private async Task<string> ReadRequestBody(HttpRequest request)
        {
            request.Body.Position = 0;
            using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            request.Body.Position = 0;
            return body;
        }

        private async Task<string> ReadResponseBody(Stream responseStream)
        {
            responseStream.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(responseStream).ReadToEndAsync();
            responseStream.Seek(0, SeekOrigin.Begin);
            return text;
        }

        private DateTime TrimToMilliseconds(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond, dt.Kind);
        }

        private string FormatDuration(TimeSpan duration)
        {
            if (duration.TotalSeconds >= 1)
                return $"{duration.TotalSeconds:F2} s";
            return $"{duration.TotalMilliseconds:F0} ms";
        }

        private static string Truncate(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value) || value.Length <= maxLength)
                return value;
            return value[..maxLength];
        }
    }
}
