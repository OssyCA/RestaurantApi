using Azure.Security.KeyVault.Secrets;
using Microsoft.EntityFrameworkCore;
using RestaurantApi.Data;

namespace RestaurantApi.Extensions
{
    public static class DatabaseExtension
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration, SecretClient secretClient)
        {

            var connectionString = secretClient.GetSecret("DbConString").Value.Value;
            services.AddDbContext<RestaurantDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnectionString"));
                //options.UseSqlServer(connectionString);
            });

            return services;
        }
    }
}
