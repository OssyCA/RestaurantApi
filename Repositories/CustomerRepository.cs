using Microsoft.EntityFrameworkCore;
using RestaurantApi.Data;
using RestaurantApi.Models;
using RestaurantApi.Repositories.IRepositories;

namespace RestaurantApi.Repositories
{
    public class CustomerRepository(RestaurantDbContext context) : ICustomerRepository
    {
        private static string? NormalizeEmail(string email) => email?.Trim().ToLowerInvariant();
        private static string? NormalizePhone(string? phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return null;
            var digits = new string([.. phone.Where(ch => char.IsDigit(ch) || ch == '+')]);
            return string.IsNullOrWhiteSpace(digits) ? null : digits;
        }
        public async Task<Customer?> GetCustomerByEmailOrPhoneAsync(string email, string phone)
        {
            var e = NormalizeEmail(email);
            var p = NormalizePhone(phone);

            return await context.Customers
                .FirstOrDefaultAsync(c =>
                    (e != null && c.Email == e) ||
                    (p != null && c.Phone == p));
        }
        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            customer.Email = NormalizeEmail(customer.Email);
            customer.Phone = NormalizePhone(customer.Phone);

            context.Customers.Add(customer);
            await context.SaveChangesAsync();
            return customer;
        }
        public async Task<Customer?> GetOrCreateCustomerAsync(string name, string email, string phone)
        {
            var e = NormalizeEmail(email);
            var p = NormalizePhone(phone);

            // First, try to get existing customer
            var existing = await GetCustomerByEmailOrPhoneAsync(e, p);
            if (existing != null)
            {
                var changed = false;
                if (existing.Name != name) { existing.Name = name; changed = true; }
                if (existing.Email != e) { existing.Email = e; changed = true; }
                if (existing.Phone != p) { existing.Phone = p; changed = true; }

                if (changed)
                {
                    await context.SaveChangesAsync();
                }

                return existing;
            }

            // If no existing customer, create new one
            var newCustomer = new Customer
            {
                Name = name,
                Email = e,
                Phone = p
            };

            try
            {
                return await CreateCustomerAsync(newCustomer);
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message?.Contains("duplicate key") == true)
            {
         
                return await GetCustomerByEmailOrPhoneAsync(e, p);
            }
        }

        public async Task<Customer?> UpdateCustomerAsync(Customer customer)
        {
            customer.Email = NormalizeEmail(customer.Email);
            customer.Phone = NormalizePhone(customer.Phone);

            context.Customers.Update(customer);

            await context.SaveChangesAsync();
            return customer;
        }

        public async Task<bool> DeleteCustomerAsync(int customerId)
        {
            var deletedRows = await context.Customers
               .Where(c => c.CustomerId == customerId)
               .ExecuteDeleteAsync();

            return deletedRows > 0;
        }
    }
}
