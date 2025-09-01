using RestaurantApi.Models;

namespace RestaurantApi.Repositories.IRepositories
{
    public interface IEmployeeRepository
    {
        Task<Employee> RegisterEmployee(Employee employee);
        Task<Employee?> GetEmployeeByEmail(string email);
        Task<Employee?> GetEmployeeById(int id);
        Task<bool> EmployeRefreshToken(Employee employee, string refreshtoken);
    }
}
