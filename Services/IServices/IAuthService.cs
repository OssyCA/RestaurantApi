using RestaurantApi.DTO;
using RestaurantApi.Models;

namespace RestaurantApi.Services.IServices
{
    public interface IAuthService
    {
        Task<Employee?> RegisterEmployeeAsync(EmployeeDTO request);
        Task<TokenResponseDTO?> LoginAsync(EmployeeLoginDTO request);
        Task<TokenResponseDTO?> RefreshTokensAsync(RequestRefreshTokenDto request);
    }
}
