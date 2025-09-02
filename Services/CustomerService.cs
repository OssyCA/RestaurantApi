using RestaurantApi.DTO;
using RestaurantApi.DTO.Common;
using RestaurantApi.Models;
using RestaurantApi.Repositories.IRepositories;
using RestaurantApi.Services.IServices;

namespace RestaurantApi.Services
{
    public class CustomerService(ICustomerRepository customerRepository) : ICustomerService
    {
        public async Task<ApiResponse<Customer>> CreateCustomerAsync(CustomerDTO dto)
        {
            var customer = new Customer
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone
            };

            var existingCustomer = await customerRepository.GetCustomerByEmailOrPhoneAsync(dto.Email, dto.Phone);
            if (existingCustomer != null)
            {
                return ApiResponse<Customer>.Error("Customer with this email or phone already exists");
            }

            try
            {
                var newCustomer = await customerRepository.CreateCustomerAsync(customer);
                return ApiResponse<Customer>.Ok(newCustomer, "Customer created successfully");
            }
            catch
            {
                return ApiResponse<Customer>.Error("Failed to create customer");
            }
        }

        public async Task<CustomerDTO?> GetCustomerByEmailOrPhoneAsync(string email, string phone)
        {
            var customer = await customerRepository.GetCustomerByEmailOrPhoneAsync(email, phone);

            if (customer == null)
                return null;

            return new CustomerDTO
            {
                Name = customer.Name,
                Email = customer.Email,
                Phone = customer.Phone
            };
        }

        public async Task<(bool Success, List<string> ErrorMessages)> UpdateCustomerAsync(int customerId, CustomerDTO dto)
        {
            var customer = await customerRepository.GetCustomerByEmailOrPhoneAsync(dto.Email, dto.Phone);

            if (customer == null)
            {
                return (false, new List<string> { "Customer not found" });
            }

            if (customer.CustomerId != customerId)
            {
                return (false, new List<string> { "Email or phone belongs to another customer" });
            }

            customer.Name = dto.Name;
            customer.Email = dto.Email;
            customer.Phone = dto.Phone;

            try
            {
                var updatedCustomer = await customerRepository.UpdateCustomerAsync(customer);
                if (updatedCustomer != null)
                {
                    return (true, new List<string>());
                }
                return (false, new List<string> { "Failed to update customer" });
            }
            catch
            {
                return (false, new List<string> { "Database error occurred" });
            }
        }

        public async Task<(bool Success, List<string> ErrorMessages)> DeleteCustomerAsync(int customerId)
        {
            var success = await customerRepository.DeleteCustomerAsync(customerId);

            if (success)
            {
                return (true, new List<string>());
            }

            return (false, new List<string> { "Customer not found or failed to delete" });
        }
    }
}