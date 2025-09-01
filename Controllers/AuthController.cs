using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestaurantApi.Data;
using RestaurantApi.DTO;
using RestaurantApi.DTO.Common;
using RestaurantApi.Helpers;
using RestaurantApi.Models;
using RestaurantApi.Services.IServices;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;

namespace RestaurantApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService service, RestaurantDbContext context) : ControllerBase
    {
        [HttpPost("RegisterEmployee")]
        public async Task<ActionResult<ApiResponse<Employee>>> EmployeeRegister(EmployeeDTO request)
        {
            var employee = await service.RegisterEmployeeAsync(request);

            if (employee == null)
                return BadRequest(ApiResponse<Employee>.Error("Email already used"));

            return Ok(ApiResponse<Employee>.Ok(employee, "Employee registered"));
        }

        [HttpPost("LoginEmployee")]
        public async Task<ActionResult<ApiResponse>> LoginEmployee(EmployeeLoginDTO request)
        {
            var tokenResponse = await service.LoginAsync(request);

            if (tokenResponse is null)
                return Unauthorized(ApiResponse.Error("Invalid username or password"));

            SetAuthenticationCookies.SetAuthenticationCookie(HttpContext, tokenResponse.AccessToken, tokenResponse.RefreshToken, context);

            return Ok(ApiResponse.Ok("Login successful"));
        }

        [HttpPost("RefreshToken")] // Dixa bättra response meddeleanden
        public async Task<ActionResult<TokenResponseDTO>> RefreshToken()
        {

            if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
            {
                return Unauthorized(ApiResponse.Error("Cant find cookie"));
            }
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var accessToken = HttpContext.Request.Cookies["accessToken"];

                if (string.IsNullOrEmpty(accessToken))
                {
                    return Unauthorized(ApiResponse.Error("Cant find accesstoken"));
                }

                var tokenWithoutValidation = tokenHandler.ReadJwtToken(accessToken); 
                var employeeClaim = tokenWithoutValidation.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(employeeClaim) || !int.TryParse(employeeClaim, out int employeeId))
                {
                    return Unauthorized(ApiResponse.Error("soemthing went wrong"));
                }


                var refreshRequest = new RequestRefreshTokenDto
                {
                    Id = employeeId,
                    RefreshToken = refreshToken
                };

                // Use the service method
                var tokenResponse = await service.RefreshTokensAsync(refreshRequest);

                if (tokenResponse == null)
                {
                    return Unauthorized(ApiResponse.Error("soemthing went wrong"));
                }

                // Set new cookies
                HttpContext.Response.Cookies.Append("accessToken", tokenResponse.AccessToken, GetCookieOptionsData.AccessTokenCookie());
                HttpContext.Response.Cookies.Append("refreshToken", tokenResponse.RefreshToken, GetCookieOptionsData.RefreshTokenEmployeeIdCookie());

                return Ok(ApiResponse.Ok("Refreshed"));
            }
            catch
            {
                return Unauthorized(ApiResponse.Error("soemthing went wrong"));
            }



            //var result = await service.RefreshTokensAsync(requestRefreshTokenDto);
            //if (result is null || result.AccessToken is null || result.RefreshToken is null)
            //{
            //    return Unauthorized("Invalid refrsh token");
            //};

            //return Ok(result);
        }
        [HttpGet("Staff")]
        [Authorize]
        public ActionResult<ApiResponse> AuthenticatedEndpoint()
        {
            return Ok(ApiResponse.Ok("Logged in"));
        }
        [HttpGet("Admin")]
        [Authorize(Roles = nameof(EmployeeRole.Admin))]
        public ActionResult<ApiResponse> AuthenticatedAdminEndpoint()
        {
            return Ok(ApiResponse.Ok("Admin in"));
        }


    }
}
