using RestaurantApi.DTO;
using RestaurantApi.Models;

namespace RestaurantApi.Services.IServices
{
    public interface IAuthService
    {
        Task<TokenResponseDTO?> LoginAsync(EmployeeLoginDTO request);
        Task<TokenResponseDTO?> RefreshTokensAsync(RequestRefreshTokenDto request);
    }
}
