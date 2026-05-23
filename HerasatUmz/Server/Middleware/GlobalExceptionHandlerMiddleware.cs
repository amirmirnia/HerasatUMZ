using Application.Common.Errors;
using Application.Common.Exceptions;
using FluentValidation;
using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace Server.Middleware
{
    /// <summary>
    /// Catches all unhandled exceptions, maps them to an <see cref="ApiError"/> with a
    /// friendly Persian message, and writes a consistent JSON body.
    /// Keep controllers free of generic <c>catch (Exception)</c> blocks.
    /// </summary>
    public class GlobalExceptionHandlerMiddleware
    {
        private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public GlobalExceptionHandlerMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionHandlerMiddleware> logger,
            IHostEnvironment env)
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
                await HandleAsync(context, ex);
            }
        }

        private async Task HandleAsync(HttpContext context, Exception ex)
        {
            var traceId = Activity.Current?.Id ?? context.TraceIdentifier;
            var (status, error) = Map(context, ex);
            error.TraceId = traceId;

            // Log full detail server-side, never to the client.
            _logger.Log(
                status >= 500 ? LogLevel.Error : LogLevel.Warning,
                ex,
                "Request {Method} {Path} failed with {ExceptionType}. TraceId={TraceId}",
                context.Request.Method, context.Request.Path, ex.GetType().Name, traceId);

            if (context.Response.HasStarted)
            {
                _logger.LogWarning("Response has already started; cannot write ApiError body.");
                return;
            }

            context.Response.Clear();
            context.Response.StatusCode = status;
            context.Response.ContentType = "application/json; charset=utf-8";

            await context.Response.WriteAsync(JsonSerializer.Serialize(error, JsonOptions));
        }

        private (int Status, ApiError Error) Map(HttpContext context, Exception ex)
        {
            switch (ex)
            {
                case ValidationException ve:
                    return ((int)HttpStatusCode.BadRequest, new ApiError
                    {
                        Code = "validation_failed",
                        Message = "اطلاعات ورودی نامعتبر است.",
                        Errors = ve.Errors
                            .GroupBy(e => e.PropertyName)
                            .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())
                    });

                case NotFoundException nf:
                    return ((int)HttpStatusCode.NotFound, new ApiError
                    {
                        Code = "not_found",
                        Message = IsFriendly(nf.Message) ? nf.Message : "اطلاعات مورد نظر یافت نشد."
                    });

                case UnauthorizedAccessException ua:
                    // If the user is already authenticated, it's an authorization (403) issue.
                    var authed = context.User?.Identity?.IsAuthenticated == true;
                    return (authed ? (int)HttpStatusCode.Forbidden : (int)HttpStatusCode.Unauthorized,
                        new ApiError
                        {
                            Code = authed ? "forbidden" : "unauthorized",
                            Message = IsFriendly(ua.Message)
                                ? ua.Message
                                : (authed ? "شما دسترسی لازم برای این عملیات را ندارید." : "ابتدا وارد سیستم شوید.")
                        });

                case InvalidOperationException io when IsFriendly(io.Message):
                    return ((int)HttpStatusCode.BadRequest, new ApiError
                    {
                        Code = "invalid_operation",
                        Message = io.Message
                    });

                case ArgumentException ae when IsFriendly(ae.Message):
                    return ((int)HttpStatusCode.BadRequest, new ApiError
                    {
                        Code = "bad_request",
                        Message = ae.Message
                    });

                default:
                    var apiError = new ApiError
                    {
                        Code = "internal_error",
                        Message = "خطای پیش‌بینی نشده در سرور رخ داد. لطفاً دوباره تلاش کنید."
                    };
                    if (_env.IsDevelopment())
                    {
                        apiError.Errors = new Dictionary<string, string[]>
                        {
                            ["exception"] = new[] { ex.GetType().FullName ?? "Exception" },
                            ["detail"] = new[] { ex.Message }
                        };
                    }
                    return ((int)HttpStatusCode.InternalServerError, apiError);
            }
        }

        /// <summary>
        /// Heuristic: an exception message is "friendly" if it looks like a Persian/user-facing string
        /// rather than an internal .NET error (e.g. "Object reference not set...").
        /// </summary>
        private static bool IsFriendly(string? message)
        {
            if (string.IsNullOrWhiteSpace(message)) return false;

            // Any Persian/Arabic character → assume the developer wrote it for users.
            foreach (var c in message)
            {
                if (c >= 0x0600 && c <= 0x06FF) return true;
                if (c >= 0xFB50 && c <= 0xFDFF) return true;
                if (c >= 0xFE70 && c <= 0xFEFF) return true;
            }
            return false;
        }
    }

    public static class GlobalExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app) =>
            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
    }
}
