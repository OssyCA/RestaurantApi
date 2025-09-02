using RestaurantApi.Data;

namespace RestaurantApi.Helpers
{
    public class SetAuthenticationCookies
    {
        public static void SetAuthenticationCookie(HttpContext httpContext, string accessToken, string refreshToken, RestaurantDbContext context)
        {
            if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
            {
                throw new ArgumentException("Tokens cannot be null or empty");
            }

            httpContext.Response.Cookies.Append("accessToken", accessToken, GetCookieOptionsData.AccessTokenCookie());
            httpContext.Response.Cookies.Append("refreshToken", refreshToken, GetCookieOptionsData.RefreshTokenEmployeeIdCookie());
        }
    }
}
