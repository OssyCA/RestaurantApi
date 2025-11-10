using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace RestaurantApi.Extensions
{
    public static class AuthenticationExtension
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration, SecretClient secretClient)
        {
            var signingKey = secretClient.GetSecret("JWT-SECRET-KEY");
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = configuration["JwtSetting:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = configuration["JwtSetting:Audience"],
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey.ToString())),
                        ValidateIssuerSigningKey = true
                    };
                    // look for accesstoken in cookies
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            // Först kolla Authorization header
                            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
                            {
                                context.Token = authHeader["Bearer ".Length..].Trim();
                            }
                            // Om ingen Authorization header, kolla cookies
                            else if (context.Request.Cookies.ContainsKey("accessToken"))
                            {
                                context.Token = context.Request.Cookies["accessToken"];
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization();

            return services;
        }
    }
}
