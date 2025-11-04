namespace Server.Helpers
{
    public static class CookieHelper
    {
        public static CookieOptions CreateCookieOptions(
            int? minutes = null,
            int? days = null,
            bool isRefresh = false,
            IConfiguration? cfg = null)
        {
            var options = new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // فقط روی HTTPS
                SameSite = SameSiteMode.Strict, // اگر frontend و backend در یک دامنه باشند
                Path = "/",
                Expires = days.HasValue
                    ? DateTimeOffset.UtcNow.AddDays(days.Value)
                    : (minutes.HasValue
                        ? DateTimeOffset.UtcNow.AddMinutes(minutes.Value)
                        : null)
            };

            return options;
        }
    }
}
