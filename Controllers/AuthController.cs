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
using System.Net.Http;

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

            SetAuthenticationCookies.SetAuthenticationCookie(this.HttpContext, tokenResponse.AccessToken, tokenResponse.RefreshToken, context);

            return Ok(ApiResponse.Ok("Login successful"));
        }

        [HttpPost("RefreshToken")] // MÅSTE FIXAAS SENARE FÖR FUNKA MED COOKIES
        public async Task<ActionResult<TokenResponseDTO>> RefreshToken(RequestRefreshTokenDto requestRefreshTokenDto)
        {
            var result = await service.RefreshTokensAsync(requestRefreshTokenDto);
            if (result is null || result.AccessToken is null || result.RefreshToken is null)
            {
                return Unauthorized("Invalid refrsh token");
            }
            ;

            return Ok(result);
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
