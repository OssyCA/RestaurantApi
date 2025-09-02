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
    public class AuthController(IAuthService service, RestaurantDbContext context, IEmployeeService employeeService) : ControllerBase
    {
        [HttpPost("RegisterEmployee")]
        public async Task<ActionResult<ApiResponse<Employee>>> EmployeeRegister(EmployeeDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<Employee>.Error("Invalid input"));
            }
            var employee = await employeeService.RegisterEmployeeAsync(request);

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

        [HttpPost("RefreshToken")] 
        public async Task<ActionResult<ApiResponse>> RefreshToken()
        {
            if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
            {
                return Unauthorized(ApiResponse.Error("No refresh token found"));
            }

            try
            {
             
                var employee = await employeeService.GetEmployeeByValidRefreshTokenAsync(refreshToken);

                if (employee == null)
                {
                    return Unauthorized(ApiResponse.Error("Invalid or expired refresh token"));
                }

                var refreshRequest = new RequestRefreshTokenDto
                {
                    Id = employee.EmployeeId,
                    RefreshToken = refreshToken
                };

                var tokenResponse = await service.RefreshTokensAsync(refreshRequest);

                if (tokenResponse == null)
                {
                    return Unauthorized(ApiResponse.Error("Token refresh failed"));
                }

                SetAuthenticationCookies.SetAuthenticationCookie(HttpContext, tokenResponse.AccessToken, tokenResponse.RefreshToken, context);

                return Ok(ApiResponse.Ok("Token refreshed successfully"));
            }
            catch (Exception)
            {
                return Unauthorized(ApiResponse.Error("Token refresh failed"));
            }
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
