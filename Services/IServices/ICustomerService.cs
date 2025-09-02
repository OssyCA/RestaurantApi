using RestaurantApi.DTO;
using RestaurantApi.DTO.Common;
using RestaurantApi.Models;

namespace RestaurantApi.Services.IServices
{
    public interface ICustomerService
    {
        Task<ApiResponse<Customer>> CreateCustomerAsync(CustomerDTO dto);
        Task<CustomerDTO?> GetCustomerByEmailOrPhoneAsync(string email, string phone);
        Task<(bool Success, List<string> ErrorMessages)> UpdateCustomerAsync(int customerId, CustomerDTO dto);
        Task<(bool Success, List<string> ErrorMessages)> DeleteCustomerAsync(int customerId);
    }
}
