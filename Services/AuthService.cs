using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestaurantApi.Data;
using RestaurantApi.DTO;
using RestaurantApi.Models;
using RestaurantApi.Repositories.IRepositories;
using RestaurantApi.Services.IServices;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RestaurantApi.Services
{
    public class AuthService(RestaurantDbContext context, IConfiguration configuration, IEmployeeRepository repository) : IAuthService
    {
        public async Task<TokenResponseDTO?> LoginAsync(EmployeeLoginDTO request)
        {
            var employee = await repository.GetEmployeeByEmail(request.Email);

            if (employee == null)
            {
                return null;
            }


            if (new PasswordHasher<Employee>().VerifyHashedPassword(employee, employee.PasswordHash, request.Password)
                == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return await CreateTokenResponse(employee);
        }
        public async Task<Employee?> RegisterEmployeeAsync(EmployeeDTO request)
        {
            var existingEmployee = await repository.GetEmployeeByEmail(request.Email);
            if (existingEmployee != null)
            {
                return null;
            }


            var employee = new Employee();
            var hashedPassword = new PasswordHasher<Employee>()
               .HashPassword(employee, request.Password);

            employee.Name = request.Name;
            employee.Email = request.Email;
            employee.PasswordHash = hashedPassword;

            await repository.RegisterEmployee(employee);

            return employee;
        } 
        private string CreateToken(Employee employee)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, employee.EmployeeId.ToString()),
                new(ClaimTypes.Name, employee.Name),
                new(ClaimTypes.Email, employee.Email),
                new(ClaimTypes.Role, employee.EmployeeRole.ToString())
            };

            var key = new SymmetricSecurityKey(
               Encoding.UTF8.GetBytes(configuration.GetValue<string>("JwtSetting:Token")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
               issuer: configuration.GetValue<string>("JwtSetting:Issuer"),
               audience: configuration.GetValue<string>("JwtSetting:Audience"),
               claims: claims,
               expires: DateTime.UtcNow.AddHours(1),
               signingCredentials: creds
           );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
        private async Task<Employee?> ValidateRefreshTokenAsync(int employeeId, string refreshToken)
        {
            var employee = await repository.GetEmployeeById(employeeId);

            if (employee is null || employee.RefreshToken != refreshToken || employee.RefreshTokenExpireTime <= DateTime.UtcNow)
            {
                return null;
            }

            return employee;

        }
        private async Task<TokenResponseDTO> CreateTokenResponse(Employee employee)
        {
            return new TokenResponseDTO { AccessToken = CreateToken(employee), RefreshToken = await GenerateAndSaveRefreshToken(employee) };
        }
        private async Task<string> GenerateAndSaveRefreshToken(Employee employee) // Change to REPO
        {
            var refreshToken = GetGenerateRefreshToken();

            if (await repository.EmployeRefreshToken(employee, refreshToken) ==false)
            {
                throw new InvalidOperationException("Failed to save refresh token to database");
            }
            return refreshToken;
        }
        private string GetGenerateRefreshToken()
        {
            var randomNum = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNum);
            return Convert.ToBase64String(randomNum);
        }
        public async Task<TokenResponseDTO?> RefreshTokensAsync(RequestRefreshTokenDto request)
        {
            var user = await ValidateRefreshTokenAsync(request.Id, request.RefreshToken);
            if (user is null)
            {
                return null;
            }
            return await CreateTokenResponse(user);
        }
    }
}
