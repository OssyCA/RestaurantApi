using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestaurantApi.DTO;
using RestaurantApi.Models;
using RestaurantApi.Repositories.IRepositories;
using RestaurantApi.Services.IServices;

namespace RestaurantApi.Services
{
    public class EmployeeService(IEmployeeRepository employeeRepository):IEmployeeService
    {
        public async Task<Employee?> RegisterEmployeeAsync(EmployeeDTO request)
        {
            var existingEmployee = await employeeRepository.GetEmployeeByEmail(request.Email);
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

            await employeeRepository.RegisterEmployee(employee);

            return employee;
        }
        public async Task<Employee?> GetEmployeeByValidRefreshTokenAsync(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
                return null;

            return await employeeRepository.GetEmployeeByRefreshToken(refreshToken);
        }

    }
}
