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

        public async Task<bool> EmployeRefreshToken(Employee employee, string refreshtoken)
        {
            try
            {
                employee.RefreshToken = refreshtoken;
                employee.RefreshTokenExpireTime = DateTime.UtcNow.AddDays(7);
                await context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
