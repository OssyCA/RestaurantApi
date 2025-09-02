using RestaurantApi.Services;
using RestaurantApi.Services.IServices;

namespace RestaurantApi.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IMenuItemService, MenuItemService>();
            services.AddScoped<IAvailabilityService, AvailabilityService>();
            services.AddScoped<IEmployeeService, EmployeeService>();

            return services;
        }
    }
}
