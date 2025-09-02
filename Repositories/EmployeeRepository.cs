using Microsoft.EntityFrameworkCore;
using RestaurantApi.Data;
using RestaurantApi.Models;
using RestaurantApi.Repositories.IRepositories;

namespace RestaurantApi.Repositories
{
    public class EmployeeRepository(RestaurantDbContext context) : IEmployeeRepository
    {
        public async Task<Employee> RegisterEmployee(Employee employee)
        {
            context.Employees.Add(employee);
            await context.SaveChangesAsync();
            return employee;
        }
        public async Task<Employee?> GetEmployeeByEmail(string email)
        {
            return await context.Employees.FirstOrDefaultAsync(e => e.Email == email);
        }
        public async Task<bool> EmployeRefreshToken(Employee employee, string refreshtoken)
        {
            employee.RefreshToken = refreshtoken;
            employee.RefreshTokenExpireTime = DateTime.UtcNow.AddDays(7);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<Employee?> GetEmployeeById(int id)
        {
            return await context.Employees.FirstOrDefaultAsync(e => e.EmployeeId == id);
        }
        public async Task<Employee?> GetEmployeeByRefreshToken(string refreshtoken)
        {
            return await context.Employees.FirstOrDefaultAsync(e => e.RefreshToken == refreshtoken);
        }
    }
}
