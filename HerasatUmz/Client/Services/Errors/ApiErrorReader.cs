using Application.Common.Errors;
using System.Net;
using System.Net.Http.Json;

namespace Client.Services.Errors
{
    /// <summary>
    /// Reads the standard <see cref="ApiError"/> JSON body from an HTTP response.
    /// Always falls back to a friendly Persian message instead of leaking raw text.
    /// </summary>
    public static class ApiErrorReader
    {
        public static async Task<ApiError> ReadAsync(HttpResponseMessage response)
        {
            try
            {
                var error = await response.Content.ReadFromJsonAsync<ApiError>();
                if (error != null && !string.IsNullOrWhiteSpace(error.Message))
                    return error;
            }
            catch
            {
                // body wasn't valid ApiError JSON — fall through to generic.
            }

            return new ApiError
            {
                Code = "unknown",
                Message = FallbackMessage(response.StatusCode)
            };
        }

        /// <summary>Convenience: just the user-facing message string.</summary>
        public static async Task<string> ReadMessageAsync(HttpResponseMessage response)
            => (await ReadAsync(response)).Message;

        private static string FallbackMessage(HttpStatusCode status) => status switch
        {
            HttpStatusCode.BadRequest => "درخواست شما نامعتبر است.",
            HttpStatusCode.Unauthorized => "ابتدا وارد سیستم شوید.",
            HttpStatusCode.Forbidden => "شما دسترسی لازم برای این عملیات را ندارید.",
            HttpStatusCode.NotFound => "اطلاعات مورد نظر یافت نشد.",
            HttpStatusCode.Conflict => "این عملیات با وضعیت فعلی سیستم سازگار نیست.",
            HttpStatusCode.RequestTimeout => "زمان درخواست به پایان رسید.",
            HttpStatusCode.TooManyRequests => "تعداد درخواست‌ها بیش از حد است. کمی صبر کنید.",
            HttpStatusCode.InternalServerError => "خطای داخلی سرور رخ داد. لطفاً بعداً تلاش کنید.",
            HttpStatusCode.BadGateway or HttpStatusCode.ServiceUnavailable or HttpStatusCode.GatewayTimeout
                => "سرور در دسترس نیست. لطفاً بعداً تلاش کنید.",
            _ => "خطایی رخ داد. لطفاً دوباره تلاش کنید."
        };
    }
}
