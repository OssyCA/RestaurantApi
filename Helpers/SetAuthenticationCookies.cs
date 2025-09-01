using RestaurantApi.Data;

namespace RestaurantApi.Helpers
{
    public class SetAuthenticationCookies
    {
        private static ILogger<SetAuthenticationCookies> _logger;
        public static void SetAuthenticationCookie(HttpContext httpContext, string accessToken, string refreshToken, RestaurantDbContext context)
        {
            try
            {
                if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
                {
                    throw new ArgumentException("Tokens cannot be null or empty");
                }

                httpContext.Response.Cookies.Append("accessToken", accessToken, GetCookieOptionsData.AccessTokenCookie());
                httpContext.Response.Cookies.Append("refreshToken", refreshToken, GetCookieOptionsData.RefreshTokenCookie());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to set authentication cookies: {Message}", ex.Message);
                throw;
            }
        }
    }
}
