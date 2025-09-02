using RestaurantApi.Repositories;
using RestaurantApi.Repositories.IRepositories;

namespace RestaurantApi.Extensions
{
    public static class RepositoryExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<ITableRepository, TableRepository>();
            services.AddScoped<IMenuItemRepository, MenuItemRepositrory>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();

            return services;
        }
    }
}
