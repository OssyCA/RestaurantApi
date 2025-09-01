using RestaurantApi.Models;

namespace RestaurantApi.Repositories.IRepositories
{
    public interface IEmployeeRepository
    {
        Task<Employee> RegisterEmployee(Employee employee);
        Task<bool> EmployeRefreshToken(Employee employee, string refreshtoken);
    }
}
