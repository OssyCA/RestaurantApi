using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using RestaurantApi.Extensions;
using System.Threading.RateLimiting;

namespace RestaurantApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var keyVaultUri = builder.Configuration["KeyVault:VaultUri"];
            var secretClient = new SecretClient(new Uri(keyVaultUri!), new DefaultAzureCredential());
            builder.Services.AddSingleton(secretClient);
            // Add services to the container
            builder.Services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter("LimitedPolicy", config =>
                {
                    config.PermitLimit = 1;
                    config.Window = TimeSpan.FromSeconds(5);
                    config.QueueLimit = 1;
                });

                
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests; // Too Many Requests status code 

            });
            builder.Services.AddControllers();
            builder.Services.AddDatabase(builder.Configuration, secretClient);
            builder.Services.AddJwtAuthentication(builder.Configuration, secretClient);
            builder.Services.AddRepositories();
            builder.Services.AddApplicationServices();
            builder.Services.AddCorsPolicy();

            // Add Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
               
            }
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();
            app.UseCors("AllowReactRestaurant");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseRateLimiter();
            app.MapControllers();
            //GLobal RateLimit
            //app.MapControllers().RequireRateLimiting("LimitedPolicy");

            app.Run();
        }
    }
}