using RestaurantApi.DTO;
using RestaurantApi.Models;

namespace RestaurantApi.Services.IServices
{
    public interface IEmployeeService
    {
        Task<Employee?> RegisterEmployeeAsync(EmployeeDTO request);
        Task<Employee?> GetEmployeeByValidRefreshTokenAsync(string refreshToken);
        Task<Employee?> GetEmployeeByIdAsync(int id);
        Task<bool> InvalidateRefreshTokenAsync(Employee employee);

    }
}
