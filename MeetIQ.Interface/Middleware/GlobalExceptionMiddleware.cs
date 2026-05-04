using MeetIQ.Application.Common.Exceptions;
using System.Text.Json;

namespace MeetIQ.Web.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger,
            IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var (statusCode, title, detail) = exception switch
            {
                NotFoundException ex => (404, "Not Found", ex.Message),
                ForbiddenException ex => (403, "Forbidden", ex.Message),
                BadRequestException ex => (400, "Bad Request", ex.Message),
                ValidationException ex => (400, "Validation Error",
                                           string.Join("; ", ex.Errors.SelectMany(e => e.Value))),
                UnauthorizedAccessException => (401, "Unauthorized", "Authentication required."),
                _ => (500, "Server Error", "An unexpected error occurred.")
            };

            // Log 500s as errors, rest as warnings
            if (statusCode == 500)
                _logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);
            else
                _logger.LogWarning("Handled exception [{Status}]: {Message}", statusCode, exception.Message);

            // API request → return JSON 
            if (IsApiRequest(context))
            {
                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";

                var json = JsonSerializer.Serialize(new
                {
                    status = statusCode,
                    title,
                    detail = _env.IsDevelopment() ? exception.ToString() : detail
                });

                await context.Response.WriteAsync(json);
                return;
            }

            // ── MVC request → store in TempData-style and redirect ───────
            context.Response.StatusCode = statusCode;

            // Pass message via HttpContext.Items so the error view can read it
            context.Items["ErrorTitle"] = title;
            context.Items["ErrorDetail"] = detail;
            context.Items["StatusCode"] = statusCode;

            // Re-execute to the right error page
            var errorPath = statusCode switch
            {
                404 => "/Error/NotFound",
                403 => "/Error/Forbidden",
                400 => "/Error/BadRequest",
                401 => "/Error/Unauthorized",
                _ => "/Error/ServerError"
            };

            context.Request.Path = errorPath;

            try
            {
                await _next(context);
            }
            catch
            {
                // Fallback — write plain HTML if error controller itself fails
                context.Response.ContentType = "text/html";
                await context.Response.WriteAsync(
                    $"<h1>{statusCode} — {title}</h1><p>{detail}</p>");
            }
        }

        private static bool IsApiRequest(HttpContext context)
            => context.Request.Path.StartsWithSegments("/api")
            || context.Request.Headers["X-Requested-With"] == "XMLHttpRequest"
            || (context.Request.Headers["Accept"].ToString().Contains("application/json")
                && !context.Request.Headers["Accept"].ToString().Contains("text/html"));
    }
}