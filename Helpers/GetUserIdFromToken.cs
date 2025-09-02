using Microsoft.EntityFrameworkCore;
using RestaurantApi.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RestaurantApi.Helpers
{
    public class GetUserIdFromToken
    {
        public static int TryGetUserIdFromToken(string? token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return 0;
            }

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                if (!tokenHandler.CanReadToken(token))
                {
                    return 0;
                }

                var jwtToken = tokenHandler.ReadJwtToken(token);
                var employeeIdFromClaims = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(employeeIdFromClaims) || !int.TryParse(employeeIdFromClaims, out int employeeId))
                {
                    return 0;
                }

                return employeeId;
            }
            catch
            {
                return 0;
            }
        }
        public static async Task<int> GetValidatedUserIdAsync(HttpContext httpContext, RestaurantDbContext context)
        {
            int employeeId = TryGetUserIdFromToken(httpContext.Request.Cookies["accessToken"]);

            if (employeeId > 0)
            {
                var employee = await context.Employees.FirstOrDefaultAsync(e => e.EmployeeId == employeeId);
                return employee != null ? employeeId : 0;
            }

            if (httpContext.Request.Cookies.TryGetValue("refreshToken", out string? refreshToken) &&
                !string.IsNullOrEmpty(refreshToken))
            {
                var employee = await context.Employees.FirstOrDefaultAsync(u =>
                    u.RefreshToken == refreshToken &&
                    u.RefreshTokenExpireTime > DateTime.UtcNow);

                return employee?.EmployeeId ?? 0;
            }

            return 0;
        }
    }
}
