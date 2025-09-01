using RestaurantApi.Models;

namespace RestaurantApi.Repositories.IRepositories
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetCustomerByEmailOrPhoneAsync(string email, string phone);
        Task<Customer> CreateCustomerAsync(Customer customer);
        Task<Customer?> GetOrCreateCustomerAsync(string name, string email, string phone);
    }
}
