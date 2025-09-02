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
            var tokenHandler = new JwtSecurityTokenHandler();
            if (!tokenHandler.CanReadToken(token))
            {
                return 0;
            }
            var jwtToken = tokenHandler.ReadJwtToken(token);
            var employeeIFromClaims = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(employeeIFromClaims) || !int.TryParse(employeeIFromClaims, out int employeeId) || employeeIFromClaims == null)
            {
                return 0;
            }
            return employeeId;
        }
        public static async Task<int> GetValidatedUserIdAsync(HttpContext httpContext, RestaurantDbContext context)
        {
            int employeeId = TryGetUserIdFromToken(httpContext.Request.Cookies["accessToken"]);

            if (employeeId == 0 &&
                httpContext.Request.Cookies.TryGetValue("employeeIFromClaims", out string? employeeIdString) &&
                int.TryParse(employeeIdString, out int cookieEmployeeId) &&
                cookieEmployeeId != 0 &&
                httpContext.Request.Cookies.TryGetValue("refreshToken", out string? refreshToken) &&
                !string.IsNullOrEmpty(refreshToken))
            {
                var employee = await context.Employees.FirstOrDefaultAsync(u =>
                    u.EmployeeId == cookieEmployeeId &&
                    u.RefreshToken == refreshToken &&
                    u.RefreshTokenExpireTime > DateTime.UtcNow);

                if (employee != null)
                {
                    employeeId = employee.EmployeeId;
                }
            }

            return employeeId;
        }
    }
}
