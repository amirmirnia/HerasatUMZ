namespace Server.Helpers
{
    public static class CookieHelper
    {
        private const string RefreshTokenPath = "/api/auth";

        public static CookieOptions CreateCookieOptions(
            int? minutes = null,
            int? days = null,
            bool isRefresh = false,
            IConfiguration? cfg = null)
        {
            return new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Path = isRefresh ? RefreshTokenPath : "/",
                Expires = days.HasValue
                    ? DateTimeOffset.UtcNow.AddDays(days.Value)
                    : (minutes.HasValue
                        ? DateTimeOffset.UtcNow.AddMinutes(minutes.Value)
                        : null)
            };
        }

        public static CookieOptions DeleteOptions(bool isRefresh = false) => new()
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Path = isRefresh ? RefreshTokenPath : "/"
        };
    }
}
